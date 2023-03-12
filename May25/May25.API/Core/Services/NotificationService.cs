using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Enums;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;
        private readonly GoogleFirebaseOptions _firebaseOptions;
        private FirebaseApp _firebaseApp;
        private FirebaseMessaging _firebaseMessaging;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper,
            ClaimsPrincipal authUser, IOptions<GoogleFirebaseOptions> firebaseOptions,
            IEmailService emailService, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
            _firebaseOptions = firebaseOptions.Value.ThrowIfNull(nameof(firebaseOptions));
            _emailService = emailService.ThrowIfNull(nameof(emailService));
            _logger = logger;

            InitializeFirebaseApp();
        }

        public async Task SendAsync(NotificationForCreationDTO nfc)
        {
            await FormatNotificationBody(nfc);

            var response = await SendToFirebase(nfc);

            await SaveToDatabase(nfc, response);

            try
            {
                await SendEmail(nfc);
            }
            catch (Exception ex)
            {
                // Yes, it fails silently but I'm in a rush
                // TODO: handle this better
                _logger.LogError($"Failed to send email: {ex.Message}");
            }
        }

        public async Task AddTokenAsync(NotificationTokenForCreationDTO tokenDto)
        {
            var userId = _authUser.GetId();
            var notificationtoken = await _unitOfWork.Notifications.GetTokenByUserId(userId);

            if (notificationtoken == null)
            {
                notificationtoken = new NotificationToken { UserId = userId };
                _unitOfWork.Notifications.AddToken(notificationtoken);
            }

            notificationtoken.Token = tokenDto.Value;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<NotificationTokenDTO>> GetAllTokensAsync()
        {
            var tokens = await _unitOfWork.Notifications.GetAllAsync();

            return _mapper.Map<IEnumerable<NotificationTokenDTO>>(tokens);
        }

        public async Task<IEnumerable<NotificationDTO>> GetUnreadAsync()
        {
            var userId = _authUser.GetId();
            var notifications = await _unitOfWork.Notifications.GetUnreadAsync(userId);

            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetAsync(id);

            if (notification == null)
            {
                throw new NotFoundException($"Notification {id} not found");
            }

            if (_authUser.GetId() != notification.TargetUserId)
            {
                throw new BadRequestException($"Notification {id} is not yours");
            }

            notification.Read = true;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllAsync()
        {
            var userId = _authUser.GetId();
            var notifications = await _unitOfWork.Notifications.GetAllForUserAsync(userId);

            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
        }

        public async Task<SendTripCompletedNotificationsResultDTO> SendTripCompletedNotificationsAsync()
        {
            var trips = await _unitOfWork.Trips.GetRecentlyCompletedAsync();
            var notificationsCount = 0;

            foreach (var trip in trips)
            {
                var passengers = trip.PassengersIds.ToIntArray();

                await SendAsync(new NotificationForCreationDTO
                {
                    Title = "Calificación pendiente",
                    Body = $"Tu viaje ha finalizado y ya puedes calificar a tus pasajeros",
                    GeneratorUserId = passengers.First(),
                    TargetUserId = trip.DriverId,
                    Type = NotificationType.TripCompleted,
                    RefId = trip.TripId
                });

                notificationsCount++;

                foreach (var passenger in passengers)
                {
                    await SendAsync(new NotificationForCreationDTO
                    {
                        Title = "Calificación pendiente",
                        Body = $"Tu viaje ha finalizado y ya puedes calificar a @@GeneratorUserName@@",
                        GeneratorUserId = trip.DriverId,
                        TargetUserId = passenger,
                        Type = NotificationType.TripCompleted,
                        RefId = trip.TripId
                    });

                    notificationsCount++;
                }
            }

            return new SendTripCompletedNotificationsResultDTO($"{notificationsCount} notification(s) sent for {trips.Count()} trip(s)");
        }

        #region Helpers
        private void InitializeFirebaseApp()
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                return;
            }

            var jsonOptions = JsonSerializer.Serialize(_firebaseOptions);

            _firebaseApp = FirebaseApp.Create(new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonOptions).CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            });

            _firebaseMessaging = FirebaseMessaging.GetMessaging(_firebaseApp);
        }

        private async Task FormatNotificationBody(NotificationForCreationDTO nfc)
        {
            if (nfc.Body.Contains("@@GeneratorUserName@@"))
            {
                string generatorUserName = await _unitOfWork.Users.GetPublicNameAsync(nfc.GeneratorUserId);
                nfc.Body = nfc.Body.Replace("@@GeneratorUserName@@", generatorUserName);
            }
        }

        private async Task<string> SendToFirebase(NotificationForCreationDTO nfc)
        {
            var targetToken = await _unitOfWork.Notifications.GetTokenByUserId(nfc.TargetUserId);

            if (string.IsNullOrEmpty(targetToken?.Token))
            {
                return await Task.FromResult("No token found");
            }

            var badge = await _unitOfWork.Notifications.GetUnreadCount(nfc.TargetUserId);

            var message = new FirebaseAdmin.Messaging.Message()
            {
                Token = targetToken.Token,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = nfc.Title,
                    Body = nfc.Body
                },
                Data = new Dictionary<string, string>() { { "badge", $"{badge + 1}" } }
            };

            return await _firebaseMessaging.SendAsync(message);
        }

        private async Task SaveToDatabase(NotificationForCreationDTO nfc, string firebaseResponse)
        {
            var notification = _mapper.Map<Models.Entities.Notification>(nfc);
            notification.CreatedAt = DateTime.Now;
            notification.FirebaseResponse = firebaseResponse;

            _unitOfWork.Notifications.Add(notification);
            await _unitOfWork.CompleteAsync();
        }

        private async Task SendEmail(NotificationForCreationDTO nfc)
        {
            var user = await _unitOfWork.Users.GetAsync(nfc.TargetUserId);

            if (!user.IsSubscribed)
            {
                return;
            }

            await _emailService.SendNotificationEmailAsync(user.Email, nfc);
        }
        #endregion
    }
}

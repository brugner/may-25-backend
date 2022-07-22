using AutoMapper;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Enums;
using May25.API.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;
        private readonly INotificationService _notificationService;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper,
            ClaimsPrincipal authUser, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
        }

        public async Task<MessageDTO> CreateAsync(MessageForCreationDTO messageForCreation)
        {
            var toUser = await _unitOfWork.Users.Exists(messageForCreation.ToUserId);

            if (!toUser)
            {
                throw new NotFoundException($"User {messageForCreation.ToUserId} not found");
            }

            var trip = await _unitOfWork.Trips.GetAsync(messageForCreation.TripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {messageForCreation.TripId} not found");
            }

            if (trip.DriverId != messageForCreation.ToUserId && !trip.Passengers.Any(x => x.PassengerId == messageForCreation.ToUserId))
            {
                throw new BadRequestException($"User {messageForCreation.ToUserId} is not part of this trip");
            }

            if (trip.Departure > DateTime.Now)
            {
                throw new BadRequestException($"Trip {messageForCreation.TripId} is already completed");
            }

            var message = _mapper.Map<Message>(messageForCreation);
            message.FromUserId = _authUser.GetId();
            message.CreatedAt = DateTime.Now;

            _unitOfWork.Messages.Add(message);

            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Nuevo mensaje",
                Body = $"@@GeneratorUserName@@ te ha enviado un mensaje",
                GeneratorUserId = message.FromUserId,
                TargetUserId = message.ToUserId,
                Type = NotificationType.MessageReceived,
                RefId = message.TripId
            });

            return _mapper.Map<MessageDTO>(message);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync(int tripId, int user1, int user2)
        {
            var messages = await _unitOfWork.Messages.GetAllAsync(tripId, user1, user2);

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }
    }
}

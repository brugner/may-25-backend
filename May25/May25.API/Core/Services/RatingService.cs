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
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;
        private readonly INotificationService _notificationService;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper,
            ClaimsPrincipal authUser, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
        }

        public async Task<RatingDTO> RateUserAsync(RatingForCreationDTO ratingForCreation)
        {
            var authUserId = _authUser.GetId();

            await ValidateRatingForCreation(ratingForCreation, authUserId);

            var rating = _mapper.Map<Rating>(ratingForCreation);
            rating.FromUserId = authUserId;
            rating.CreatedAt = DateTime.Now;

            _unitOfWork.Ratings.Add(rating);

            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Calificación",
                Body = $"@@GeneratorUserName@@ te ha calificado",
                GeneratorUserId = authUserId,
                TargetUserId = rating.ToUserId,
                Type = NotificationType.RatingReceived,
                RefId = rating.ToUserId
            });

            return _mapper.Map<RatingDTO>(rating);
        }

        public async Task<RatingDTO> GetRatingAsync(int ratingId)
        {
            var rating = await _unitOfWork.Ratings.GetAsync(ratingId);

            if (rating == null)
            {
                throw new NotFoundException($"Rating {ratingId} not found");
            }

            return _mapper.Map<RatingDTO>(rating);
        }

        public async Task<RatingDTO> ReplyToRatingAsync(int ratingId, ReplyToRatingDTO replyToRating)
        {
            var authUserId = _authUser.GetId();
            var rating = await _unitOfWork.Ratings.GetAsync(ratingId);

            if (rating == null)
            {
                throw new NotFoundException($"Rating {ratingId} not found");
            }

            if (rating.ToUserId != authUserId)
            {
                throw new BadRequestException($"Users can only reply to ratings they have received");
            }

            rating.Reply = replyToRating.Reply;
            rating.RepliedAt = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Calificación",
                Body = $"@@GeneratorUserName@@ ha respondido tu calificación",
                GeneratorUserId = authUserId,
                TargetUserId = rating.FromUserId,
                Type = NotificationType.RatingReplied,
                RefId = rating.ToUserId
            });

            return _mapper.Map<RatingDTO>(rating);
        }

        public async Task<UserRatingsDTO> GetUserRatingAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"User {userId} not found");
            }

            var ratings = await _unitOfWork.Ratings.GetAllForUserAsync(userId);

            var asDriver = _mapper.Map<IEnumerable<RatingDTO>>(ratings.Where(x => x.ToUserType == UserType.Driver).OrderByDescending(x => x.CreatedAt));
            var starsAsDriver = asDriver.Any() ? asDriver.Average(x => x.Stars) : 0;

            var asPassenger = _mapper.Map<IEnumerable<RatingDTO>>(ratings.Where(x => x.ToUserType == UserType.Passenger).OrderByDescending(x => x.CreatedAt));
            var starsAsPassenger = asPassenger.Any() ? asPassenger.Average(x => x.Stars) : 0;

            return new UserRatingsDTO
            {
                User = _mapper.Map<UserPublicProfileDTO>(user),
                RatingsAsDriver = asDriver,
                StarsAsDriver = Math.Round(starsAsDriver, 2),
                RatingsAsPassenger = asPassenger,
                StarsAsPassenger = Math.Round(starsAsPassenger, 2)
            };
        }

        public async Task<RatingDTO> RatingExistsAsync(RatingExistsParamsDTO ratingExist)
        {
            var authUserId = _authUser.GetId();
            var rating = await _unitOfWork.Ratings.GetAsync(authUserId, ratingExist.ToUserId, ratingExist.TripId);

            return _mapper.Map<RatingDTO>(rating);
        }

        #region Helpers
        private async Task ValidateRatingForCreation(RatingForCreationDTO ratingForCreation, int authUserId)
        {
            var ratingExists = await _unitOfWork.Ratings.Exists(authUserId, ratingForCreation.ToUserId, ratingForCreation.TripId);

            // The rating was already done
            if (ratingExists)
            {
                throw new BadRequestException($"User already rated for this trip");
            }

            var trip = await _unitOfWork.Trips.GetAsync(ratingForCreation.TripId);

            // The trip doesn't exists
            if (trip == null)
            {
                throw new NotFoundException($"Trip {ratingForCreation.TripId} not found");
            }

            if (DateTime.Now < trip.Departure.AddHours(24))
            {
                throw new BadRequestException($"User can only rate trips 24hs after the departure");
            }

            var toUserExists = await _unitOfWork.Users.Exists(ratingForCreation.ToUserId);

            // The user doesn't exists
            if (!toUserExists)
            {
                throw new NotFoundException($"User {ratingForCreation.ToUserId} not found");
            }

            // The 'from' user wasn't part of the trip
            if (trip.DriverId != authUserId && !trip.Passengers.Any(x => x.PassengerId == authUserId))
            {
                throw new BadRequestException($"Users can only rate trips they were part of");
            }

            // The 'to' user wasn't part of the trip
            if (trip.DriverId != ratingForCreation.ToUserId && !trip.Passengers.Any(x => x.PassengerId == ratingForCreation.ToUserId))
            {
                throw new BadRequestException($"Users can only rate users they shared a trip with");
            }
        }
        #endregion
    }
}

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
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;
        private readonly IPlaceService _placeService;
        private readonly INotificationService _notificationService;

        public TripService(IUnitOfWork unitOfWork, IMapper mapper, ClaimsPrincipal user,
            IPlaceService placeService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = user.ThrowIfNull(nameof(user));
            _placeService = placeService.ThrowIfNull(nameof(placeService));
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
        }

        public async Task CancelSeatAsync(int tripId)
        {
            var userId = _authUser.GetId();
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            var tripPassenger = trip.Passengers.SingleOrDefault(x => x.PassengerId == userId);

            if (tripPassenger == null)
            {
                throw new NotFoundException($"You are not a passenger of trip {tripId}");
            }

            await _unitOfWork.Trips.RemovePassenger(tripId, userId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CancelTripAsync(int tripId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (_authUser.GetId() != trip.DriverId)
            {
                throw new BadRequestException($"You are not the driver of trip {tripId}");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            trip.IsCanceled = true;
            trip.CanceledAt = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            await NotifyUsersTripIsCancelled(trip);
        }

        public async Task<TripDTO> CreateAsync(TripForCreationDTO tripForCreation)
        {
            var trip = _mapper.Map<Trip>(tripForCreation);
            trip.DriverId = _authUser.GetId();
            trip.CreatedAt = DateTime.Now;
            trip.Origin.ValidUntil = DateTime.Now.AddDays(30);
            trip.Destination.ValidUntil = DateTime.Now.AddDays(30);

            _unitOfWork.Trips.Add(trip);
            await _unitOfWork.CompleteAsync();

            await NotifyUsersWithAlerts(trip);

            return await GetTripAsync(trip.Id);
        }

        public async Task<IEnumerable<TripDTO>> GetAvailableTripsAsync()
        {
            var trips = await _unitOfWork.Trips.GetAvailableTripsAsync();

            return _mapper.Map<IEnumerable<TripDTO>>(trips);
        }

        public async Task<MyTripsDTO> GetMyTripsAsync()
        {
            var userId = _authUser.GetId();
            var asDriver = await _unitOfWork.Trips.GetUserTripsAsync(userId, UserAs.Driver);
            var asPassenger = await _unitOfWork.Trips.GetUserTripsAsync(userId, UserAs.Passenger);
            var asSeatRequester = await _unitOfWork.Trips.GetUserTripsAsync(userId, UserAs.SeatRequester);

            foreach (var trip in asDriver)
            {
                await _placeService.UpdatePlaceCacheAsync(trip.Origin);
                await _placeService.UpdatePlaceCacheAsync(trip.Destination);
            }

            foreach (var trip in asPassenger)
            {
                await _placeService.UpdatePlaceCacheAsync(trip.Origin);
                await _placeService.UpdatePlaceCacheAsync(trip.Destination);
            }

            foreach (var trip in asSeatRequester)
            {
                await _placeService.UpdatePlaceCacheAsync(trip.Origin);
                await _placeService.UpdatePlaceCacheAsync(trip.Destination);
            }

            return new MyTripsDTO
            {
                AsDriver = _mapper.Map<IEnumerable<TripSummaryDTO>>(asDriver),
                AsPassenger = _mapper.Map<IEnumerable<TripSummaryDTO>>(asPassenger),
                AsSeatRequester = _mapper.Map<IEnumerable<TripSummaryDTO>>(asSeatRequester)
            };
        }

        public async Task<TripDTO> GetTripAsync(int tripId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            // Only the driver can see the seat requests for the trip
            if (_authUser.GetId() != trip.DriverId)
            {
                trip.SeatRequests = null;
            }

            await _placeService.UpdatePlaceCacheAsync(trip.Origin);
            await _placeService.UpdatePlaceCacheAsync(trip.Destination);

            return _mapper.Map<TripDTO>(trip);
        }

        public async Task<IEnumerable<TripSummaryDTO>> SearchAsync(TripsSearchParamsDTO searchParams)
        {
            var trips = await _unitOfWork.Trips.Search(searchParams);

            return _mapper.Map<IEnumerable<TripSummaryDTO>>(trips);
        }

        #region Helpers
        private async Task NotifyUsersTripIsCancelled(Trip trip)
        {
            foreach (var passenger in trip.Passengers)
            {
                await _notificationService.SendAsync(new NotificationForCreationDTO
                {
                    Title = "Viaje cancelado",
                    Body = $"@@GeneratorUserName@@ ha cancelado el viaje",
                    GeneratorUserId = _authUser.GetId(),
                    TargetUserId = passenger.Id,
                    Type = NotificationType.TripCanceled,
                    RefId = trip.Id
                });
            }

            foreach (var seatRequest in trip.SeatRequests)
            {
                await _notificationService.SendAsync(new NotificationForCreationDTO
                {
                    Title = "Viaje cancelado",
                    Body = $"@@GeneratorUserName@@ ha cancelado el viaje",
                    GeneratorUserId = _authUser.GetId(),
                    TargetUserId = seatRequest.PassengerId,
                    Type = NotificationType.TripCanceled,
                    RefId = trip.Id
                });
            }
        }

        private async Task NotifyUsersWithAlerts(Trip trip)
        {
            var alerts = await _unitOfWork.Alerts.GetAlertsAsync(trip.Origin, trip.Destination);

            foreach (var alert in alerts)
            {
                await _notificationService.SendAsync(new NotificationForCreationDTO
                {
                    Title = "Nuevo viaje",
                    Body = $"Hay un nuevo viaje que coincide con una de tus alertas",
                    GeneratorUserId = trip.DriverId,
                    TargetUserId = alert.UserId,
                    Type = NotificationType.TripCreated,
                    RefId = trip.Id
                });
            }
        }
        #endregion
    }
}

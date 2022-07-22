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
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class SeatRequestService : ISeatRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public SeatRequestService(IUnitOfWork unitOfWork, IMapper mapper, ClaimsPrincipal authUser,
            IUserService userService, ITripService tripService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
            _userService = userService.ThrowIfNull(nameof(userService));
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
        }

        public async Task<IEnumerable<TripSeatRequestDTO>> GetSeatRequestsForTripAsync(int tripId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (_authUser.GetId() != trip.Driver.Id)
            {
                throw new ForbiddenException($"You are not the driver for the trip {tripId}");
            }

            var requests = await _unitOfWork.SeatRequests.GetForTripAsync(tripId);

            return _mapper.Map<IEnumerable<TripSeatRequestDTO>>(requests);
        }

        public async Task<IEnumerable<UserSeatRequestDTO>> GetMySeatRequestsAsync()
        {
            var passengerId = _authUser.GetId();
            var requests = await _unitOfWork.SeatRequests.GetForPassengerAsync(passengerId);

            return _mapper.Map<IEnumerable<UserSeatRequestDTO>>(requests);
        }

        public async Task SendSeatRequestAsync(int tripId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            if (trip.IsCanceled)
            {
                throw new BadRequestException($"Trip {tripId} is canceled");
            }

            if (trip.MaxPassengers == trip.Passengers.Count)
            {
                throw new BadRequestException($"Trip {tripId} is full");
            }

            var request = new SeatRequest
            {
                TripId = tripId,
                PassengerId = _authUser.GetId()
            };

            _unitOfWork.SeatRequests.Add(request);

            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Solicitud de lugar",
                Body = $"@@GeneratorUserName@@ te solicita un lugar en tu viaje",
                GeneratorUserId = _authUser.GetId(),
                TargetUserId = trip.DriverId,
                Type = NotificationType.SeatRequestSent,
                RefId = trip.Id
            });
        }

        public async Task CancelSeatRequestAsync(int tripId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            if (trip.IsCanceled)
            {
                throw new BadRequestException($"Trip {tripId} is canceled");
            }

            var request = await _unitOfWork.SeatRequests.GetAsync(tripId, _authUser.GetId());

            if (request == null)
            {
                throw new NotFoundException($"Seat request for trip {tripId} not found");
            }

            _unitOfWork.SeatRequests.Remove(request);
            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Cancelación de solicitud",
                Body = $"@@GeneratorUserName@@ canceló la solicitud de lugar en tu viaje",
                GeneratorUserId = _authUser.GetId(),
                TargetUserId = trip.DriverId,
                Type = NotificationType.SeatRequestCanceled,
                RefId = trip.Id
            });
        }

        public async Task AcceptSeatRequestAsync(int tripId, int passengerId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            if (trip.IsCanceled)
            {
                throw new BadRequestException($"Trip {tripId} is canceled");
            }

            var passenger = await _userService.GetByIdAsync(passengerId);

            if (passenger == null)
            {
                throw new NotFoundException($"Passenger {passengerId} not found");
            }

            var request = await _unitOfWork.SeatRequests.GetAsync(tripId, passengerId);

            if (request == null)
            {
                throw new NotFoundException($"Seat request for trip {tripId} and passenger {passengerId} not found");
            }

            if (_authUser.GetId() != trip.Driver.Id)
            {
                throw new ForbiddenException($"You are not the driver for the trip {tripId}");
            }

            _unitOfWork.SeatRequests.Remove(request);
            _unitOfWork.Trips.AddPassenger(trip, passengerId);

            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Solicitud de lugar aceptada",
                Body = $"@@GeneratorUserName@@ aceptó tu solicitud de lugar en su viaje",
                GeneratorUserId = _authUser.GetId(),
                TargetUserId = passengerId,
                Type = NotificationType.SeatRequestAccepted,
                RefId = trip.Id
            });
        }

        public async Task RejectSeatRequestAsync(int tripId, int passengerId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(tripId);

            if (trip == null)
            {
                throw new NotFoundException($"Trip {tripId} not found");
            }

            if (trip.Departure < DateTime.Now)
            {
                throw new BadRequestException($"Trip {tripId} already departed");
            }

            if (trip.IsCanceled)
            {
                throw new BadRequestException($"Trip {tripId} is canceled");
            }

            var passenger = await _userService.GetByIdAsync(passengerId);

            if (passenger == null)
            {
                throw new NotFoundException($"Passenger {passengerId} not found");
            }

            var request = await _unitOfWork.SeatRequests.GetAsync(tripId, passengerId);

            if (request == null)
            {
                throw new NotFoundException($"Seat request for trip {tripId} and passenger {passengerId} not found");
            }

            if (_authUser.GetId() != trip.Driver.Id)
            {
                throw new ForbiddenException($"You are not the driver for the trip {tripId}");
            }

            _unitOfWork.SeatRequests.Remove(request);
            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(new NotificationForCreationDTO
            {
                Title = "Solicitud de lugar rechazada",
                Body = $"@@GeneratorUserName@@ rechazó tu solicitud de lugar en su viaje",
                GeneratorUserId = _authUser.GetId(),
                TargetUserId = passengerId,
                Type = NotificationType.SeatRequestRejected,
                RefId = trip.Id
            });
        }
    }
}

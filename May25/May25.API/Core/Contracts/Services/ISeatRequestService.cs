using May25.API.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface ISeatRequestService
    {
        Task<IEnumerable<UserSeatRequestDTO>> GetMySeatRequestsAsync();
        Task<IEnumerable<TripSeatRequestDTO>> GetSeatRequestsForTripAsync(int tripId);
        Task SendSeatRequestAsync(int tripId);
        Task CancelSeatRequestAsync(int tripId);
        Task AcceptSeatRequestAsync(int tripId, int passengerId);
        Task RejectSeatRequestAsync(int tripId, int passengerId);
    }
}

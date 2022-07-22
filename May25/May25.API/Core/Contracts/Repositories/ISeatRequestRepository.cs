using May25.API.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface ISeatRequestRepository : IRepository<SeatRequest, int>
    {
        Task<IEnumerable<SeatRequest>> GetForTripAsync(int tripId);
        Task<IEnumerable<SeatRequest>> GetForPassengerAsync(int passengerId);
        Task<SeatRequest> GetAsync(int tripId, int passengerId);
    }
}

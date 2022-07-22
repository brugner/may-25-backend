using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class SeatRequestRepository : Repository<SeatRequest, int>, ISeatRequestRepository
    {
        public SeatRequestRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<SeatRequest> GetAsync(int tripId, int passengerId)
        {
            return await AppDbContext.SeatRequests
                .SingleOrDefaultAsync(x => x.TripId == tripId && x.PassengerId == passengerId);
        }

        public async Task<IEnumerable<SeatRequest>> GetForTripAsync(int tripId)
        {
            return await AppDbContext.SeatRequests.Where(x => x.TripId == tripId).ToListAsync();
        }

        public async Task<IEnumerable<SeatRequest>> GetForPassengerAsync(int passengerId)
        {
            return await AppDbContext.SeatRequests.Where(x => x.PassengerId == passengerId).ToListAsync();
        }
    }
}

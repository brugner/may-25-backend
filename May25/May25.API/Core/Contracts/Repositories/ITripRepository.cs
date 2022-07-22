using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Entities.Keyless;
using May25.API.Core.Models.Enums;
using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface ITripRepository : IRepository<Trip, int>
    {
        Task<IEnumerable<Trip>> Search(TripsSearchParamsDTO searchParams);
        void AddPassenger(Trip trip, int passengerId);
        Task<IEnumerable<Trip>> GetUserTripsAsync(int userId, UserAs userAs);
        Task RemovePassenger(int tripId, int userId);
        Task<IEnumerable<Trip>> GetAvailableTripsAsync();
        Task<IEnumerable<RecentlyCompletedTrip>> GetRecentlyCompletedAsync();
        Task<IEnumerable<Trip>> GetTripsReadyForGoogleCacheCleanUp();
    }
}

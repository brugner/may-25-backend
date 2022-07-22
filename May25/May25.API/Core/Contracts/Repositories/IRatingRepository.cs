using May25.API.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IRatingRepository : IRepository<Rating, int>
    {
        Task<bool> Exists(int fromUserId, int toUserId, int tripId);
        Task<IEnumerable<Rating>> GetAllForUserAsync(int userId);
        Task<Rating> GetAsync(int fromUserId, int toUserId, int tripId);
    }
}

using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class RatingRepository : Repository<Rating, int>, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<bool> Exists(int fromUserId, int toUserId, int tripId)
        {
            return await AppDbContext.Ratings
                .SingleOrDefaultAsync(x => x.FromUserId == fromUserId && x.ToUserId == toUserId && x.TripId == tripId) != null;
        }

        public async Task<IEnumerable<Rating>> GetAllForUserAsync(int userId)
        {
            return await AppDbContext.Ratings
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .Where(x => x.ToUserId == userId).ToListAsync();
        }

        public async Task<Rating> GetAsync(int fromUserId, int toUserId, int tripId)
        {
            return await AppDbContext.Ratings
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .SingleOrDefaultAsync(x => x.FromUserId == fromUserId && x.ToUserId == toUserId && x.TripId == tripId);
        }
    }
}

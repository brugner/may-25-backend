using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class MessageRepository : Repository<Message, int>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Message>> GetAllAsync(int tripId, int user1, int user2)
        {
            return await AppDbContext.Messages
                .Where(x => x.TripId == tripId && (x.FromUserId == user1 && x.ToUserId == user2 || x.FromUserId == user2 && x.ToUserId == user1))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}

using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class NotificationRepository : Repository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {

        }

        public void AddToken(NotificationToken token)
        {
            AppDbContext.NotificationTokens.Add(token);
        }

        public async Task<IEnumerable<Notification>> GetAllForUserAsync(int userId)
        {
            return await AppDbContext.Notifications
                .Where(x => x.TargetUserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<NotificationToken> GetTokenByUserId(int userId)
        {
            return await AppDbContext.NotificationTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Notification>> GetUnreadAsync(int targetUserId)
        {
            return await AppDbContext.Notifications
                .Where(x => x.TargetUserId == targetUserId && !x.Read)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCount(int targetUserId)
        {
            return await AppDbContext.Notifications.CountAsync(x => x.TargetUserId == targetUserId && !x.Read);
        }
    }
}

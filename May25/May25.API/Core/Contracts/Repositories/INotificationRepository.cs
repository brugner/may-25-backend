using May25.API.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface INotificationRepository : IRepository<Notification, int>
    {
        void AddToken(NotificationToken token);
        Task<NotificationToken> GetTokenByUserId(int userId);
        Task<int> GetUnreadCount(int targetUserId);
        Task<IEnumerable<Notification>> GetUnreadAsync(int userId);
        Task<IEnumerable<Notification>> GetAllForUserAsync(int userId);
    }
}

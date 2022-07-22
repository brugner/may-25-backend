using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface INotificationService
    {
        Task AddTokenAsync(NotificationTokenForCreationDTO notificationToken);
        Task<IEnumerable<NotificationTokenDTO>> GetAllTokensAsync();
        Task SendAsync(NotificationForCreationDTO nfc);
        Task<IEnumerable<NotificationDTO>> GetUnreadAsync();
        Task MarkAsReadAsync(int id);
        Task<IEnumerable<NotificationDTO>> GetAllAsync();
        Task<SendTripCompletedNotificationsResultDTO> SendTripCompletedNotificationsAsync();
    }
}

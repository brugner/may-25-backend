using May25.API.Core.Models.Resources;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
        Task SendConfirmationEmailAsync(ConfirmEmailParamsDTO data);
        Task SendNotificationEmailAsync(string email, NotificationForCreationDTO nfc);
        Task SendPasswordResetRequestAsync(string email, string token);
    }
}

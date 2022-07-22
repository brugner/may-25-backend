using May25.API.Core.Models.Resources;

namespace May25.API.Core.Contracts.Services
{
    public interface IHtmlService
    {
        string GetEmailConfirmationError(string email);
        string GetEmailConfirmationSuccess(string email);
        string GetEmailUnsubscribed();
        string GetConfirmationEmail(ConfirmEmailParamsDTO data);
        string GetNotificationEmail(string email, string title, string body);
        string GetPasswordResetRequest(string email, string token);
        string GetPasswordResetForm(string email, string token);
    }
}

using May25.API.Core.Models.Resources;

namespace May25.API.Core.Contracts.Services
{
    public interface IUrlService
    {
        string GetUnsubscribeUrl(string email);
        string GetConfirmationUrl(ConfirmEmailParamsDTO data);
        string GetPasswordResetUrl(string email, string token);
    }
}

using May25.API.Core.Models.Resources;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IAuthService
    {
        Task<UserAuthenticationResultDTO> AuthenticateAsync(UserForAuthenticationDTO userForAuth);
        Task<UserAuthenticationResultDTO> AuthenticateGoogleAsync(string token);
        Task<ClientAuthenticationResultDTO> AuthenticateAsync(ClientForAuthenticationDTO clientForAuth);
    }
}

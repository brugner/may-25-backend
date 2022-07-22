using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IUserService
    {
        Task<CreatedUserDTO> CreateAsync(UserForCreationDTO userForCreation);
        Task<bool> UserExists(string email);
        Task<UserDTO> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> UpdateAsync(int id, UserForUpdateDTO userForUpdate);
        Task<UserPublicProfileDTO> GetUserPublicProfileAsync(int userId);
        Task<HtmlContentResultDTO> ConfirmEmailAsync(ConfirmEmailParamsDTO data);
        Task<HtmlContentResultDTO> UnsubscribeAsync(string email);
        Task ChangePasswordAsync(ChangePasswordDTO changePassword);
        Task RequestPasswordResetAsync(string email);
        HtmlContentResultDTO GetPasswordResetForm(string email, string token);
        Task ResetPasswordAsync(string email, string token, ResetPasswordDTO data);
    }
}

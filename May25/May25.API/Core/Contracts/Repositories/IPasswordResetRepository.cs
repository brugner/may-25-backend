using May25.API.Core.Models.Entities;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IPasswordResetRepository : IRepository<PasswordReset, int>
    {
        Task<PasswordReset> GetAsync(string email, string token);
    }
}

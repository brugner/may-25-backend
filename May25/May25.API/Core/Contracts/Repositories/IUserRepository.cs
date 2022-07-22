using May25.API.Core.Models.Entities;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> Exists(int id);
        Task<string> GetPublicNameAsync(int id);
    }
}

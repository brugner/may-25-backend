using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class PasswordResetRepository : Repository<PasswordReset, int>, IPasswordResetRepository
    {
        public PasswordResetRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<PasswordReset> GetAsync(string email, string token)
        {
            return await AppDbContext.PasswordResets
                .SingleOrDefaultAsync(x => x.Email.Equals(email) && x.Token.Equals(token));
        }
    }
}

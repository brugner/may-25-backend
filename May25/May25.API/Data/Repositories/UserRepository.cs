using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await AppDbContext.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email.Equals(email));
        }

        public override async ValueTask<User> GetAsync(int id)
        {
            return await AppDbContext.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await AppDbContext.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .ToListAsync();
        }

        public void AddNotificationToken(NotificationToken token)
        {
            AppDbContext.NotificationTokens.Add(token);
        }

        public async Task<bool> Exists(int id)
        {
            return await AppDbContext.Users.SingleOrDefaultAsync(x => x.Id == id) != null;
        }

        public async Task<string> GetPublicNameAsync(int id)
        {
            var user = await AppDbContext.Users
                .Where(x => x.Id == id)
                .Select(x => new { x.FirstName, x.LastName })
                .FirstOrDefaultAsync();

            return user.FirstName + " " + user.LastName.FirstOrDefault() + ".";
        }
    }
}

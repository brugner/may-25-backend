using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class CarRepository : Repository<Car, int>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context)
        {

        }

        public override async ValueTask<Car> GetAsync(int id)
        {
            return await AppDbContext
                 .Cars
                 .Include(x => x.Make)
                 .Include(x => x.Model)
                 .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Car>> GetUserCarsAsync(int userId)
        {
            return await AppDbContext
                .Cars
                .Include(x => x.Make)
                .Include(x => x.Model)
                .Where(x => x.DriverId == userId && !x.IsDeleted)
                .ToListAsync();
        }
    }
}

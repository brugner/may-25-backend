using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class MakeRepository : Repository<Make, int>, IMakeRepository
    {
        public MakeRepository(AppDbContext context) : base(context)
        {

        }

        public async override Task<IEnumerable<Make>> GetAllAsync()
        {
            var makes = await AppDbContext.Makes
                .Include(x => x.Models)
                .OrderBy(x => x.Name)
                .ToListAsync();

            foreach (var make in makes)
            {
                make.Models = make.Models.OrderBy(x => x.Name).ToList();
            }

            return makes;
        }
    }
}

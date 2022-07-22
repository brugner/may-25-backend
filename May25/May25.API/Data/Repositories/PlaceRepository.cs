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
    public class PlaceRepository : Repository<Place, int>, IPlaceRepository
    {
        public PlaceRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Place>> GetPlacesReadyForGoogleCacheCleanUp()
        {
            return await AppDbContext.Places
                .Where(x => x.ValidUntil.Value.Date == DateTime.Today)
                .ToListAsync();
        }
    }
}

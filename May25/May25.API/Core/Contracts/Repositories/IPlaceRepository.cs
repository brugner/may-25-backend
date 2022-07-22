﻿using May25.API.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IPlaceRepository : IRepository<Place, int>
    {
        Task<IEnumerable<Place>> GetPlacesReadyForGoogleCacheCleanUp();
    }
}

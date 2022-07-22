using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Helpers;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class AlertRepository : Repository<Alert, int>, IAlertRepository
    {
        public AlertRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(Place origin, Place destination)
        {
            var alerts = await AppDbContext.Alerts.ToListAsync();

            return alerts
                .Where(x => GeoHelper.ArePointsNear(x.OriginLat, x.OriginLng, origin.Lat.Value, origin.Lng.Value) &&
                            GeoHelper.ArePointsNear(x.DestinationLat, x.DestinationLng, destination.Lat.Value, destination.Lng.Value));
        }

        public async Task<IEnumerable<Alert>> GetAlertsReadyForGoogleCacheCleanUp()
        {
            return await AppDbContext.Alerts
                .Where(x => x.ValidUntil.Date == DateTime.Today)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetAllAsync(int userId)
        {
            return await AppDbContext.Alerts
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Alert> GetAsync(int id, int userId)
        {
            return await AppDbContext.Alerts
                .SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }
    }
}

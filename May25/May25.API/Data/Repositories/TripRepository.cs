using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Helpers;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Entities.Keyless;
using May25.API.Core.Models.Enums;
using May25.API.Core.Models.Resources;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Data.Repositories
{
    public class TripRepository : Repository<Trip, int>, ITripRepository
    {
        public TripRepository(AppDbContext context) : base(context)
        {

        }

        public void AddPassenger(Trip trip, int passengerId)
        {
            trip.Passengers.Add(new TripPassenger() { TripId = trip.Id, PassengerId = passengerId });
        }

        public async override Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await AppDbContext.Trips
                .Include(x => x.Driver)
                .Include(x => x.Origin)
                .Include(x => x.Destination)
                .Where(x => !x.IsCanceled)
                .ToListAsync();
        }

        public async override ValueTask<Trip> GetAsync(int id)
        {
            return await AppDbContext.Trips
                 .Include(x => x.Driver)
                 .Include(x => x.Car)
                 .Include(x => x.Origin)
                 .Include(x => x.Destination)
                 .Include(x => x.SeatRequests)
                    .ThenInclude(x => x.Passenger)
                 .Include(x => x.Passengers)
                    .ThenInclude(x => x.Passenger)
                 .SingleOrDefaultAsync(x => x.Id == id && !x.IsCanceled);
        }

        public async Task<IEnumerable<Trip>> GetAvailableTripsAsync()
        {
            return await AppDbContext.Trips
                 .Include(x => x.Driver)
                 .Include(x => x.Origin)
                 .Include(x => x.Destination)
                 .Where(x => !x.IsCanceled && x.Departure.Date >= DateTime.Today)
                 .ToListAsync();
        }

        public async Task<IEnumerable<RecentlyCompletedTrip>> GetRecentlyCompletedAsync()
        {
            return await AppDbContext.RecentlyCompletedTrips
                .FromSqlRaw(@"
SELECT 
    Trips.Id AS 'TripId', 
    Trips.DriverId, 
    (SELECT group_concat(TripPassengers.PassengerId) FROM TripPassengers WHERE TripPassengers.TripId = Trips.Id) AS 'PassengersIds'
FROM Trips
WHERE Trips.IsCanceled = 0 AND 
    DATETIME(Trips.Departure, '+24 hours', 'utc') <= DATETIME('now', 'utc') 
    AND DATETIME(Trips.Departure, '+48 hours', 'utc') > DATETIME('now', 'utc')
    AND (SELECT COUNT(Id) FROM Notifications WHERE Notifications.Type = 5 AND Notifications.RefId = Trips.Id) = 0")
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsReadyForGoogleCacheCleanUp()
        {
            return await AppDbContext.Trips
                .Where(x => x.CreatedAt < DateTime.UtcNow.AddDays(-30) && x.CreatedAt > DateTime.UtcNow.AddDays(-40))
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetUserTripsAsync(int userId, UserAs userAs)
        {
            var query = AppDbContext.Trips.Include(x => x.Origin).Include(x => x.Destination).AsQueryable();

            query = userAs switch
            {
                UserAs.Driver => query.Where(x => x.DriverId == userId),
                UserAs.Passenger => query.Where(x => x.Passengers.Any(k => k.PassengerId == userId)),
                UserAs.SeatRequester => query.Where(x => x.SeatRequests.Any(k => k.PassengerId == userId)),
                _ => throw new ArgumentException(),
            };

            var trips = await query.ToListAsync();
            return trips.OrderByDescending(x => x.Departure).ToList();
        }

        public async Task RemovePassenger(int tripId, int passengerId)
        {
            var tripPassenger = await AppDbContext.TripPassengers.
                SingleOrDefaultAsync(x => x.TripId == tripId && x.PassengerId == passengerId);

            AppDbContext.TripPassengers.Remove(tripPassenger);
        }

        public async Task<IEnumerable<Trip>> Search(TripsSearchParamsDTO search)
        {
            var trips = await GetAvailableTripsAsync();

            return trips
               .Where(trip =>
                   trip.Departure.Date == search.Departure.Date &&
                   GeoHelper.ArePointsNear(trip.Origin.Lat.GetValueOrDefault(), trip.Origin.Lng.GetValueOrDefault(), search.Origin.Latitude, search.Origin.Longitude) &&
                   GeoHelper.ArePointsNear(trip.Destination.Lat.GetValueOrDefault(), trip.Destination.Lng.GetValueOrDefault(), search.Destination.Latitude, search.Destination.Longitude))
               .ToList();
        }
    }
}

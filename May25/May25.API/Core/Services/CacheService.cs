using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class CacheService : ICacheService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CacheService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
        }

        public async Task<CleanGoogleApiCacheResultDTO> CleanGoogleMapsApiCacheAsync()
        {
            // Places
            var places = await _unitOfWork.Places.GetPlacesReadyForGoogleCacheCleanUp();

            foreach (var place in places)
            {
                place.Lat = null;
                place.Lng = null;
                place.FormattedAddress = null;
                place.ValidUntil = null;
            }

            // Alerts
            var alerts = await _unitOfWork.Alerts.GetAlertsReadyForGoogleCacheCleanUp();
            _unitOfWork.Alerts.RemoveRange(alerts);

            // Trips
            var trips = await _unitOfWork.Trips.GetTripsReadyForGoogleCacheCleanUp();

            foreach (var trip in trips)
            {
                trip.Distance = null;
                trip.Duration = null;
            }

            await _unitOfWork.CompleteAsync();

            return new CleanGoogleApiCacheResultDTO($"{places.Count()} place(s) and {alerts.Count()} alert(s) cleaned from the cache");
        }
    }
}

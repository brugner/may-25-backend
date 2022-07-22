using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IPlaceService
    {
        Task<IEnumerable<PlaceAutocompleteDTO>> QueryAutocompleteAsync(string searchTerm);
        Task<IEnumerable<PlaceAutocompleteDTO>> SearchNearbyAsync(double latitude, double longitude);
        Task<PlaceDetailDTO> GetPlaceDetailsAsync(string id);
        Task UpdatePlaceCacheAsync(Place place);
    }
}

using May25.API.Core.Contracts.Services;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/places")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet("search-by")]
        public async Task<ActionResult<IEnumerable<PlaceAutocompleteDTO>>> QueryAutocomplete(string searchTerm)
        {
            var places = await _placeService.QueryAutocompleteAsync(searchTerm);

            return Ok(places);
        }

        [HttpGet("search-nearby")]
        public async Task<ActionResult<IEnumerable<PlaceAutocompleteDTO>>> QueryAutocomplete(double lat, double lng)
        {
            var places = await _placeService.SearchNearbyAsync(lat, lng);

            return Ok(places);
        }

        [HttpGet("details")]
        public async Task<ActionResult<PlaceDetailDTO>> GetDetails(string id)
        {
            var place = await _placeService.GetPlaceDetailsAsync(id);

            return Ok(place);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }
    }
}

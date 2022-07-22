using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cache")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService.ThrowIfNull(nameof(cacheService));
        }

        /// <summary>
        /// Cleans the Google Maps API Cache according to https://cloud.google.com/maps-platform/terms/maps-service-terms.
        /// </summary>
        /// <returns></returns>
        [HttpPost("clean-google-maps")]
        [Authorize(Roles = AppRoles.ApiClient)]
        public async Task<ActionResult<CleanGoogleApiCacheResultDTO>> CleanGoogleMapsApiCache()
        {
            var result = await _cacheService.CleanGoogleMapsApiCacheAsync();

            return Ok(result);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "POST");
            return Ok();
        }
    }
}

using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingService;

        public SettingsController(ISettingsService settingService)
        {
            _settingService = settingService.ThrowIfNull(nameof(settingService));
        }

        [HttpGet]
        public ActionResult<SettingsDTO> GetSettings()
        {
            var settings = _settingService.GetAll();

            return Ok(settings);
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

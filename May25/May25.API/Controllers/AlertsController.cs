using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;

        public AlertsController(IAlertService alertService)
        {
            _alertService = alertService.ThrowIfNull(nameof(alertService));
        }

        /// <summary>
        /// Get all alerts of the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertDTO>>> GetMyAlerts()
        {
            var alerts = await _alertService.GetMyAlerts();

            return Ok(alerts);
        }

        /// <summary>
        /// Get an alert.
        /// </summary>
        /// <param name="id">Alert Id.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAlert")]
        public async Task<ActionResult<IEnumerable<AlertDTO>>> GetAlert(int id)
        {
            var alert = await _alertService.GetAlertAsync(id);

            return Ok(alert);
        }

        /// <summary>
        /// Create a new alert.
        /// </summary>
        /// <param name="alertForCreation">Alert data.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AlertDTO>> CreateAlert([FromBody] AlertForCreationDTO alertForCreation)
        {
            var alert = await _alertService.CreateAsync(alertForCreation);

            return CreatedAtRoute("GetAlert", new { id = alert.Id }, alert);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            await _alertService.DeleteAlertAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,DELETE");
            return Ok();
        }
    }
}

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
    [Route("api/seatrequests")]
    public class SeatRequestsController : ControllerBase
    {
        private readonly ISeatRequestService _seatRequestService;

        public SeatRequestsController(ISeatRequestService seatRequestService)
        {
            _seatRequestService = seatRequestService;
        }

        /// <summary>
        /// Send a seat request from the authenticated user for the specified trip.
        /// </summary>
        /// <param name="tripId">Id of the trip.</param>
        /// <returns></returns>
        [HttpPost("trip/{tripId}/send")]
        public async Task<IActionResult> SendSeatRequest(int tripId)
        {
            await _seatRequestService.SendSeatRequestAsync(tripId);

            return NoContent();
        }

        /// <summary>
        /// Cancel a seat request from the authenticated user in the specified trip.
        /// </summary>
        /// <param name="tripId">Id of the trip.</param>
        /// <returns></returns>
        [HttpPost("trip/{tripId}/cancel")]
        public async Task<IActionResult> CancelSeatRequest(int tripId)
        {
            await _seatRequestService.CancelSeatRequestAsync(tripId);

            return NoContent();
        }

        /// <summary>
        /// Accept the seat request by the specified passenger for the specified trip.
        /// The authenticated user must be the driver for trip.
        /// </summary>
        /// <param name="tripId">Id of the trip.</param>
        /// <param name="passengerId">Id of the passenger.</param>
        /// <returns></returns>
        [HttpPost("trip/{tripId}/passenger/{passengerId}/accept")]
        public async Task<IActionResult> AcceptSeatRequest(int tripId, int passengerId)
        {
            await _seatRequestService.AcceptSeatRequestAsync(tripId, passengerId);

            return NoContent();
        }

        /// <summary>
        /// Reject the seat request by the specified passenger for the specified trip.
        /// The authenticated user must be the driver for trip.
        /// </summary>
        /// <param name="tripId">Id of the trip.</param>
        /// <param name="passengerId">Id of the passenger.</param>
        /// <returns></returns>
        [HttpPost("trip/{tripId}/passenger/{passengerId}/reject")]
        public async Task<IActionResult> RejectSeatRequest(int tripId, int passengerId)
        {
            await _seatRequestService.RejectSeatRequestAsync(tripId, passengerId);

            return NoContent();
        }

        /// <summary>
        /// Returns all the seat requests made by the authenticated user as a passenger.
        /// </summary>
        /// <returns></returns>
        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<UserSeatRequestDTO>>> GetMySeatRequests()
        {
            var requests = await _seatRequestService.GetMySeatRequestsAsync();

            return Ok(requests);
        }

        /// <summary>
        /// Returns all the seat requests made for the specified trip.
        /// The authenticated user must be the driver for trip.
        /// </summary>
        /// <param name="tripId">Id of the trip.</param>
        /// <returns></returns>
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<TripSeatRequestDTO>>> GetSeatRequestsForTrip(int tripId)
        {
            var requests = await _seatRequestService.GetSeatRequestsForTripAsync(tripId);

            return Ok(requests);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET,POST");
            return Ok();
        }
    }
}

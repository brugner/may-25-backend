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
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService.ThrowIfNull(nameof(tripService));
        }

        /// <summary>
        /// Gets a trip.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{tripId}", Name = "GetTrip")]
        public async Task<ActionResult<TripDTO>> GetTrip(int tripId)
        {
            var trip = await _tripService.GetTripAsync(tripId);

            return Ok(trip);
        }

        /// <summary>
        /// Gets the authenticated user trips.
        /// </summary>
        /// <returns></returns>
        [HttpGet("mine")]
        public async Task<ActionResult<MyTripsDTO>> GetMyTrips()
        {
            var trips = await _tripService.GetMyTripsAsync();

            return Ok(trips);
        }

        /// <summary>
        /// Gets all the available trips.
        /// </summary>
        /// <returns></returns>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetAvailableTrips()
        {
            var trips = await _tripService.GetAvailableTripsAsync();

            return Ok(trips);
        }

        /// <summary>
        /// Creates a new trip.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TripDTO>> CreateTrip([FromBody] TripForCreationDTO tripForCreation)
        {
            var trip = await _tripService.CreateAsync(tripForCreation);

            return CreatedAtRoute("GetTrip", new { tripId = trip.Id }, trip);
        }

        /// <summary>
        /// Searches for trips.
        /// </summary>
        /// <param name="searchParams">Trips search params.</param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<TripDTO>>> SearchTrips([FromBody] TripsSearchParamsDTO searchParams)
        {
            var trips = await _tripService.SearchAsync(searchParams);

            return Ok(trips);
        }

        /// <summary>
        /// Cancel a seat from the authenticated user in the specified trip.
        /// The authenticated user must be a passenger in the trip.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{tripId}/cancel-seat")]
        public async Task<IActionResult> CancelSeat(int tripId)
        {
            await _tripService.CancelSeatAsync(tripId);

            return NoContent();
        }

        /// <summary>
        /// Cancel the specified trip.
        /// The authenticated user must be the driver of the trip.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{tripId}/cancel")]
        public async Task<IActionResult> Cancel(int tripId)
        {
            await _tripService.CancelTripAsync(tripId);

            return NoContent();
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

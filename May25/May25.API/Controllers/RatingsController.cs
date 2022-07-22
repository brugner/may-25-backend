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
    [Route("api/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService.ThrowIfNull(nameof(ratingService));
        }

        /// <summary>
        /// Get a rating.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ratingId}", Name = "GetRating")]
        public async Task<ActionResult<CarDTO>> GetRating(int ratingId)
        {
            var rating = await _ratingService.GetRatingAsync(ratingId);

            return Ok(rating);
        }

        /// <summary>
        /// Rates a user.
        /// </summary>
        /// <param name="ratingForCreation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<RatingDTO>> RateUser(RatingForCreationDTO ratingForCreation)
        {
            var rating = await _ratingService.RateUserAsync(ratingForCreation);

            return CreatedAtRoute("GetRating", new { ratingId = rating.Id }, rating);
        }

        /// <summary>
        /// Reply to a rating.
        /// </summary>
        /// <param name="ratingId"></param>
        /// <param name="replyToRating"></param>
        /// <returns></returns>
        [HttpPost("{ratingId}/reply")]
        public async Task<ActionResult<RatingDTO>> ReplyToRating(int ratingId, ReplyToRatingDTO replyToRating)
        {
            var rating = await _ratingService.ReplyToRatingAsync(ratingId, replyToRating);

            return Ok(rating);
        }

        /// <summary>
        /// Get the ratings received by the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<UserRatingsDTO>> GetUserRating(int userId)
        {
            var result = await _ratingService.GetUserRatingAsync(userId);

            return Ok(result);
        }

        /// <summary>
        /// If the authenticated user has already rated the specified user for the specified trip, returns the rating.
        /// </summary>
        /// <param name="ratingExist">Trip and user to check.</param>
        /// <returns></returns>
        [HttpPost("exists")]
        public async Task<ActionResult<RatingDTO>> GetUserRating(RatingExistsParamsDTO ratingExist)
        {
            var result = await _ratingService.RatingExistsAsync(ratingExist);

            return Ok(result);
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

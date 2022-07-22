using May25.API.Core.Models.Resources;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IRatingService
    {
        Task<RatingDTO> RateUserAsync(RatingForCreationDTO ratingForCreation);
        Task<RatingDTO> GetRatingAsync(int ratingId);
        Task<RatingDTO> ReplyToRatingAsync(int ratingId, ReplyToRatingDTO replyToRating);
        Task<UserRatingsDTO> GetUserRatingAsync(int userId);
        Task<RatingDTO> RatingExistsAsync(RatingExistsParamsDTO ratingExist);
    }
}

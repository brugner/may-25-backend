using System.Collections.Generic;

namespace May25.API.Core.Models.Resources
{
    public class UserRatingsDTO
    {
        public UserPublicProfileDTO User { get; set; }
        public IEnumerable<RatingDTO> RatingsAsDriver { get; set; }
        public double StarsAsDriver { get; set; }
        public IEnumerable<RatingDTO> RatingsAsPassenger { get; set; }
        public double StarsAsPassenger { get; set; }
    }
}

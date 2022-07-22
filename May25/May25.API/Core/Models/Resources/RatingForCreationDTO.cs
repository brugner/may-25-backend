using May25.API.Core.Models.Enums;

namespace May25.API.Core.Models.Resources
{
    public class RatingForCreationDTO
    {
        public int TripId { get; set; }
        public int ToUserId { get; set; }
        public UserType ToUserType { get; set; }
        public byte Stars { get; set; }
        public string Comment { get; set; }
    }
}

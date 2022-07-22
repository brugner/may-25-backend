using May25.API.Core.Models.Enums;
using System;

namespace May25.API.Core.Models.Resources
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public UserPublicProfileDTO FromUser { get; set; }
        public UserPublicProfileDTO ToUser { get; set; }
        public UserType ToUserType { get; set; }
        public string Comment { get; set; }
        public string Reply { get; set; }
        public byte Stars { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RepliedAt { get; set; }
    }
}

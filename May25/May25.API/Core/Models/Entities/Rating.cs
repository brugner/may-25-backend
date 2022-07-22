using May25.API.Core.Models.Enums;
using System;

namespace May25.API.Core.Models.Entities
{
    public class Rating : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int FromUserId { get; set; }
        public User FromUser { get; set; }
        public int ToUserId { get; set; }
        public User ToUser { get; set; }
        public UserType ToUserType { get; set; }
        public byte Stars { get; set; }
        public string Comment { get; set; }
        public string Reply { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RepliedAt { get; set; }
    }
}

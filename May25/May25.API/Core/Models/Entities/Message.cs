using System;

namespace May25.API.Core.Models.Entities
{
    public class Message : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int FromUserId { get; set; }
        public User FromUser { get; set; }
        public int ToUserId { get; set; }
        public User ToUser { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

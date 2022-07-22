using System;

namespace May25.API.Core.Models.Resources
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

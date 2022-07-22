using May25.API.Core.Models.Enums;
using System;

namespace May25.API.Core.Models.Entities
{
    public class Notification : BaseEntity
    {
        public int GeneratorUserId { get; set; }
        public User GeneratorUser { get; set; }
        public int TargetUserId { get; set; }
        public User TargetUser { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public int? RefId { get; set; }
        public bool Read { get; set; }
        public string FirebaseResponse { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

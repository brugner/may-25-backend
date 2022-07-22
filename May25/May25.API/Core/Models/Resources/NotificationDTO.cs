using May25.API.Core.Models.Enums;
using System;

namespace May25.API.Core.Models.Resources
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int GeneratorUserId { get; set; }
        public int TargetUserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public int? RefId { get; set; }
        public bool Read { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

using May25.API.Core.Models.Enums;

namespace May25.API.Core.Models.Resources
{
    public class NotificationForCreationDTO
    {
        public int GeneratorUserId { get; set; }
        public int TargetUserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public int RefId { get; set; }
    }
}

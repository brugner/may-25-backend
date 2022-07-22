namespace May25.API.Core.Models.Entities
{
    public class NotificationToken : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}

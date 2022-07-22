namespace May25.API.Core.Models.Resources
{
    public class MessageForCreationDTO
    {
        public int TripId { get; set; }
        public int ToUserId { get; set; }
        public string Text { get; set; }
    }
}

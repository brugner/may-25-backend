namespace May25.API.Core.Models.Resources
{
    public class SendTripCompletedNotificationsResultDTO
    {
        public string Message { get; set; }

        public SendTripCompletedNotificationsResultDTO()
        {

        }

        public SendTripCompletedNotificationsResultDTO(string message)
        {
            Message = message;
        }
    }
}

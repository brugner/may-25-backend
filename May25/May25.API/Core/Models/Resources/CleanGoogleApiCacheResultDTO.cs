namespace May25.API.Core.Models.Resources
{
    public class CleanGoogleApiCacheResultDTO
    {
        public string Message { get; set; }

        public CleanGoogleApiCacheResultDTO()
        {

        }

        public CleanGoogleApiCacheResultDTO(string message)
        {
            Message = message;
        }
    }
}

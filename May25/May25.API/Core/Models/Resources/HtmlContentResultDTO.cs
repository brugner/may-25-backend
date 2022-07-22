namespace May25.API.Core.Models.Resources
{
    public class HtmlContentResultDTO
    {
        public bool Success { get; set; }
        public string Content { get; set; }

        public HtmlContentResultDTO()
        {

        }

        public HtmlContentResultDTO(bool success, string content)
        {
            Success = success;
            Content = content;
        }
    }
}

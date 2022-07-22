namespace May25.API.Core.Options
{
    public class EmailOptions
    {
        public string SmtpClient { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }
    }
}

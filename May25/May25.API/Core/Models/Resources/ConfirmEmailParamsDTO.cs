namespace May25.API.Core.Models.Resources
{
    public class ConfirmEmailParamsDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public ConfirmEmailParamsDTO()
        {

        }

        public ConfirmEmailParamsDTO(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}

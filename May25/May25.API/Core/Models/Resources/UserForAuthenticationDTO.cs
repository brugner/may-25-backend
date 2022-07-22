using System.ComponentModel.DataAnnotations;

namespace May25.API.Core.Models.Resources
{
    public class UserForAuthenticationDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

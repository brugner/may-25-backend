using System.ComponentModel.DataAnnotations;

namespace May25.API.Core.Models.Resources
{
    public class UserForCreationDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

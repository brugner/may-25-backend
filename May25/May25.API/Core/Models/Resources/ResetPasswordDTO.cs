using System.ComponentModel.DataAnnotations;

namespace May25.API.Core.Models.Resources
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string RepeatPassword { get; set; }
    }
}

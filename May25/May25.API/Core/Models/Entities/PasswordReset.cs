using System;

namespace May25.API.Core.Models.Entities
{
    public class PasswordReset : BaseEntity
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
    }
}

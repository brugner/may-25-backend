using System;
using System.Collections.Generic;

namespace May25.API.Core.Models.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public byte Talk { get; set; }
        public byte Music { get; set; }
        public byte Pets { get; set; }
        public byte Smoking { get; set; }
        public string EmailConfirmationToken { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<UserRoles> Roles { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}

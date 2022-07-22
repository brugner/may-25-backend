using System;
using System.Collections.Generic;

namespace May25.API.Core.Models.Resources
{
    public class CreatedUserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

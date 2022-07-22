using System;

namespace May25.API.Core.Models.Resources
{
    public class UserForUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public byte? Talk { get; set; }
        public byte? Music { get; set; }
        public byte? Pets { get; set; }
        public byte? Smoking { get; set; }
    }
}

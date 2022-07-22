using System;

namespace May25.API.Core.Models.Resources
{
    public class UserPublicProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public byte Talk { get; set; }
        public byte Music { get; set; }
        public byte Pets { get; set; }
        public byte Smoking { get; set; }
        public UserRatingsDTO Ratings { get; set; }
    }
}

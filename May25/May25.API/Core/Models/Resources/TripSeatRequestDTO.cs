using System;

namespace May25.API.Core.Models.Resources
{
    public class TripSeatRequestDTO
    {
        public int Id { get; set; }
        public UserPublicProfileDTO Passenger { get; set; }
    }
}

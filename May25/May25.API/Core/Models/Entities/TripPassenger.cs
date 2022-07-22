using System;

namespace May25.API.Core.Models.Entities
{
    public class TripPassenger : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int PassengerId { get; set; }
        public User Passenger { get; set; }
    }
}

namespace May25.API.Core.Models.Entities
{
    public class SeatRequest : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int PassengerId { get; set; }
        public User Passenger { get; set; }
    }
}

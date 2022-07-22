namespace May25.API.Core.Models.Entities.Keyless
{
    public class RecentlyCompletedTrip
    {
        public int TripId { get; set; }
        public int DriverId { get; set; }
        public string PassengersIds { get; set; }
    }
}

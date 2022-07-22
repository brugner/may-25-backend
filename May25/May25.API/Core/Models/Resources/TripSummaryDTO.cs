using System;

namespace May25.API.Core.Models.Resources
{
    public class TripSummaryDTO
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
    }
}

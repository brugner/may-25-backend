using System;

namespace May25.API.Core.Models.Resources
{
    public class TripsSearchParamsDTO
    {
        public TripsSearchParamsPlaceDTO Origin { get; set; }
        public TripsSearchParamsPlaceDTO Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
    }
}

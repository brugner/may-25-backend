using System.Collections.Generic;

namespace May25.API.Core.Models.Resources
{
    public class MyTripsDTO
    {
        public IEnumerable<TripSummaryDTO> AsDriver { get; set; }
        public IEnumerable<TripSummaryDTO> AsPassenger { get; set; }
        public IEnumerable<TripSummaryDTO> AsSeatRequester { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace May25.API.Core.Models.Resources
{
    public class TripDTO
    {
        public int Id { get; set; }
        public UserPublicProfileDTO Driver { get; set; }
        public CarDTO Car { get; set; }
        public int OriginId { get; set; }
        public PlaceDTO Origin { get; set; }
        public int DestinationId { get; set; }
        public PlaceDTO Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public byte MaxPassengers { get; set; }
        public string Description { get; set; }
        public int? Distance { get; set; }
        public int? Duration { get; set; }
        public int SuggestedCost { get; set; }
        public int Cost { get; set; }
        public int CostPerPassenger { get; set; }
        public IEnumerable<TripSeatRequestDTO> SeatRequests { get; set; }
        public IEnumerable<UserPublicProfileDTO> Passengers { get; set; }
    }
}

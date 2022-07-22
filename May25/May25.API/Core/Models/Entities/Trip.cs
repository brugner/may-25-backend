using System;
using System.Collections.Generic;

namespace May25.API.Core.Models.Entities
{
    public class Trip : BaseEntity
    {
        public int DriverId { get; set; }
        public User Driver { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int OriginId { get; set; }
        public Place Origin { get; set; }
        public int DestinationId { get; set; }
        public Place Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public byte MaxPassengers { get; set; }
        public string Description { get; set; }
        public int? Distance { get; set; }
        public int? Duration { get; set; }
        public int SuggestedCost { get; set; }
        public int Cost { get; set; }
        public int CostPerPassenger { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsCanceled { get; set; }
        public DateTimeOffset? CanceledAt { get; set; }
        public ICollection<SeatRequest> SeatRequests { get; set; }
        public ICollection<TripPassenger> Passengers { get; set; }
    }
}

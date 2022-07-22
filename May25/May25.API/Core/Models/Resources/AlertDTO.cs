using System;

namespace May25.API.Core.Models.Resources
{
    public class AlertDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OriginFormattedAddress { get; set; }
        public double OriginLat { get; set; }
        public double OriginLng { get; set; }
        public string DestinationFormattedAddress { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
    }
}

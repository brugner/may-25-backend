using System;

namespace May25.API.Core.Models.Entities
{
    public class Place : BaseEntity
    {
        public string GooglePlaceId { get; set; }
        public string FormattedAddress { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}

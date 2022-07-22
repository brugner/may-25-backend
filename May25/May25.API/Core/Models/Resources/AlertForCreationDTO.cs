namespace May25.API.Core.Models.Resources
{
    public class AlertForCreationDTO
    {
        public string OriginFormattedAddress { get; set; }
        public double OriginLat { get; set; }
        public double OriginLng { get; set; }
        public string DestinationFormattedAddress { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }
    }
}

namespace May25.API.Core.Models.Resources
{
    public class PlaceForCreationDTO
    {
        public string GooglePlaceId { get; set; }
        public string FormattedAddress { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
namespace May25.API.Core.Models.Resources
{
    public class CarForCreationDTO
    {
        public string PlateNumber { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public short Year { get; set; }
        public byte Color { get; set; }
    }
}

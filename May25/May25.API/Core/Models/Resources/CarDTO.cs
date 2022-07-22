using System;

namespace May25.API.Core.Models.Resources
{
    public class CarDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public int MakeId { get; set; }
        public string Make { get; set; }
        public int ModelId { get; set; }
        public string Model { get; set; }
        public short Year { get; set; }
        public byte Color { get; set; }
    }
}

using System;

namespace May25.API.Core.Models.Entities
{
    public class Car : BaseEntity
    {
        public int DriverId { get; set; }
        public User Driver { get; set; }
        public int MakeId { get; set; }
        public Make Make { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public string PlateNumber { get; set; }
        public byte Color { get; set; }
        public short Year { get; set; }
        public bool IsDeleted { get; set; }
    }
}

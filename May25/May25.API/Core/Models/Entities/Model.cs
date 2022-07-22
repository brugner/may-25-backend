using System;

namespace May25.API.Core.Models.Entities
{
    public class Model : BaseEntity
    {
        public int MakeId { get; set; }
        public Make Make { get; set; }
        public string Name { get; set; }
    }
}
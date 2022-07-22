using System.Collections.Generic;

namespace May25.API.Core.Models.Entities
{
    public class Make : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Model> Models { get; set; }
    }
}

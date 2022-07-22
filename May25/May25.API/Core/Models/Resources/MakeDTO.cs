using System;
using System.Collections.Generic;

namespace May25.API.Core.Models.Resources
{
    public class MakeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ModelDTO> Models { get; set; }
    }
}

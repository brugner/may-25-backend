using System.Collections.Generic;

namespace May25.API.Core.Models.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UserRoles> Users { get; set; }
    }
}

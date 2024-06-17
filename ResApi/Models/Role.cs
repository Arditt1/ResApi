using ResApi.Models.Shared;
using System.Collections.Generic;

namespace ResApi.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            Employees = new HashSet<Employee>();
            Permissions = new HashSet<Permission>();
        }

        public string? RoleName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}

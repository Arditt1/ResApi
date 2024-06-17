using ResApi.Models.Shared;
using System.Collections.Generic;

namespace ResApi.Models
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
            TableWaiters = new HashSet<TableWaiter>();
        }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ContactInfo { get; set; }
        public bool? Status { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<TableWaiter> TableWaiters { get; set; }
    }
}

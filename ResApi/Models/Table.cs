using System;
using System.Collections.Generic;
using ResApi.Models.Shared;

namespace ResApi.Models
{
    public partial class Table : BaseEntity
    {
        public Table()
        {
            Orders = new HashSet<Order>();
            TableWaiters = new HashSet<TableWaiter>();
        }

        public int? TableNumber { get; set; }
        public int? Seats { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<TableWaiter> TableWaiters { get; set; }
    }
}

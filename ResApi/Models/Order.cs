using ResApi.Models.Shared;
using System;
using System.Collections.Generic;

namespace ResApi.Models
{
    public partial class Order :BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int? TableId { get; set; }
        public int? WaiterId { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; }

        public virtual Table? Table { get; set; }
        public virtual Employee? Waiter { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

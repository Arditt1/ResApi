using ResApi.Models.Shared;
using System.Collections.Generic;

namespace ResApi.Models
{
    public partial class MenuItem : BaseEntity
    {
        public MenuItem()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }

        public virtual CategoryMenu? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

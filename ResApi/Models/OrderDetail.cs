using ResApi.Models.Shared;

namespace ResApi.Models
{
    public partial class OrderDetail :BaseEntity
    {
        public int? OrderId { get; set; }
        public int? MenuItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual MenuItem? MenuItem { get; set; }
        public virtual Order? Order { get; set; }
    }
}

using ResApi.Models.Shared;

namespace ResApi.DTO
{
    public class OrderDetailDTO
    {
        public int? OrderId { get; set; }
        public int? MenuItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}

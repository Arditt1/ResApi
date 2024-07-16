using System;
namespace ResApi.DTO.OrderDetail
{
	public class OrderFoodDTO
	{
        public int? OrderId { get; set; }
        public int? MenuItemId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderTime { get; set; }
        public decimal? OrderPrice { get; set; }
    }
}


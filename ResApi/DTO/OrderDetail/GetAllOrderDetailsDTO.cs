using System;
namespace ResApi.DTO.OrderDetail
{
	public class GetAllOrderDetailsDTO
	{
		public int? Id { get; set; }
		public int? OrderId { get; set; }
		public int? MenuItemId { get; set; }
		public int CategoryId { get; set; }
		public decimal? Price { get; set; }
		public int? TableId { get; set; }
		public int? WaiterId { get; set; }
	}
}


using System;
using System.Collections.Generic;
using ResApi.Models;

namespace ResApi.DTO.OrderDetail
{
	public class GetAllOrderDetailsDTO
	{
		public int? Id { get; set; }
		public int? OrderId { get; set; }
		public List<MenuItemDTO> MenuItems { get; set; }
		public string? CategoryName { get; set; }
		public decimal? TotalPrice { get; set; }
		public decimal? OrderPrice { get; set; }
		public int? TableNr { get; set; }
		public string? WaiterUsername { get; set; }
		public int Quantity { get; set; }
		public string? Status { get; set; }
		public DateTime? OrderTime { get; set; }

		public int CategoryId { get; set; }
		public int? WaiterId { get; set; }
		public int? MenuItemId { get; set; }
		public int? TableId { get; set; }
		public string? MenuItemName { get; set; }


	}
}


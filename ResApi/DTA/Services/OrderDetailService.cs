using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class OrderDetailService : BaseService<OrderDetail>,IOrderDetail
    {
		public OrderDetailService(DataContext context) : base(context)
		{
		}
	}
}


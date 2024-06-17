using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class OrderService : BaseService<Order>, IOrder
    {
		public OrderService(DataContext context) :base(context)
		{
		}
	}
}


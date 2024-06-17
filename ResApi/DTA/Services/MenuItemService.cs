using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class MenuItemService : BaseService<MenuItem>, IMenuItem
    {
		public MenuItemService(DataContext context) :base(context)
		{
		}
	}
}


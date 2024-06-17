using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class TableWaiterService : BaseService<TableWaiter>, ITableWaiter
    {
		public TableWaiterService(DataContext context) : base(context)
		{
		}
	}
}


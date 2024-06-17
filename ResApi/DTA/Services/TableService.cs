using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class TableService : BaseService<Table> , ITable
    {
		public TableService(DataContext context) : base(context)
		{
		}
	}
}


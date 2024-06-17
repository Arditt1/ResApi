using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class RoleService : BaseService<Role>, IRole
    {
		public RoleService(DataContext context) : base(context)
		{
		}
	}
}


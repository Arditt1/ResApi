﻿using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class PermissionService : BaseService<Permission>, IPermission
    {
		public PermissionService(DataContext context) : base(context)
		{
		}
	}
}


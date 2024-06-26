using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
	public interface IRole : IBaseService<Role>
    {
		Task<DataResponse<string>> CreateRole(RoleDTO model);
		Task<DataResponse<string>> UpdateRole(RoleDTO model);
		Task<List<RoleDTO>> GetEmployeesByRole(string roleName);
	}
}


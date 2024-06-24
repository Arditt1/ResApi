using System;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
	public interface ICategoryMenu : IBaseService<CategoryMenu>
	{
		Task<DataResponse<string>> Register(CategoryMenuDTO model);

		Task<DataResponse<string>> UpdateMenuItem(CategoryMenuDTO model);
		Task<DataResponse<string>> Add(CategoryMenuDTO entity);
	}
}


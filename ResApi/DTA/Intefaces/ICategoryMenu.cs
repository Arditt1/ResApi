﻿using System;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
	public interface ICategoryMenu : IBaseService<CategoryMenu>
	{
		Task<DataResponse<string>> CreateCategoryMenu(CategoryMenu model);

		Task<DataResponse<string>> UpdateCategoryMenu(CategoryMenuDTO entity);
	}
}


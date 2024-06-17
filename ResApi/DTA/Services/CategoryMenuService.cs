using System;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class CategoryMenuService : BaseService<CategoryMenu>, ICategoryMenu
    {
        public CategoryMenuService(DataContext context) : base(context)
        {
        }
    }
}


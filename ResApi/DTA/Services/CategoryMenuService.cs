using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class CategoryMenuService : BaseService<CategoryMenu>, ICategoryMenu
    {
        private readonly ILogger<CategoryMenuService> _logger;
        private readonly ICategoryMenu _cat;
        private readonly DataContext _context;
        public CategoryMenuService(DataContext context, ILogger<CategoryMenuService> logger) : base(context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<DataResponse<string>> Register(CategoryMenuDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfUserExists = await _context.CategoryMenus
                       .AnyAsync(x => x.CategoryName == model.CategoryName);
            if (checkIfUserExists)
            {
                response.ErrorMessage = "Kategoria me emrin: " + model.CategoryName + " ekziston";
                return response;
            }

            try
            {
                CategoryMenuDTO categoryMenu = new CategoryMenuDTO();
                categoryMenu.CategoryName = model.CategoryName;
                categoryMenu.Photo = model.Photo;

                CategoryMenu category = Extentions.AutoMapperProfile.MapForRegisterCategory(model);
                _cat.Add(category);
                // Adding the user to context of users.
                if (categoryMenu != null)
                {
                    response.Succeeded = true;
                    response.Data = "succes";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin.";
                _logger.LogError(e, $"CreateCustomerProfile: On adding user in customer portal db error {e.Message}");
            }
            return response;
        }
    }
}


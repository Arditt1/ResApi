using System;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoryMenuService(DataContext context, ILogger<CategoryMenuService> logger,IMapper mapper) : base(context)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public bool Add(CategoryMenuDTO entity)
        {
            try
            {
                if (entity != null)
                {
                    var catMapped = _mapper.Map<CategoryMenu>(entity);
                    _context.CategoryMenus.Add(catMapped);
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<DataResponse<string>> Register(CategoryMenuDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfUserExists = await _context.CategoryMenus
                       .AnyAsync(x => x.CategoryName == model.CategoryName);
            if (checkIfUserExists)
            {
                response.ErrorMessage = "Kategoria me emrin: " + model.CategoryName + " ekziston";
                response.Succeeded = false;
                return response;
            }

            try
            {
                CategoryMenuDTO categoryMenu = new CategoryMenuDTO();
                categoryMenu.CategoryName = model.CategoryName;

                var entity = _mapper.Map<CategoryMenu>(categoryMenu);
                _cat.Add(entity);
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


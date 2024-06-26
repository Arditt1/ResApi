using System;
using System.Linq;
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

        public async Task<DataResponse<string>> CreateCategoryMenu(CategoryMenu entity)
        {
            try
            {
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

                if (entity != null)
                {
                    var categoryExists = await _context.CategoryMenus.AnyAsync(x => x.CategoryName == entity.CategoryName);
                    if(!categoryExists)
                    {
                        response.Succeeded = false;
                        response.Data = "Exists";
                        return response;
                    }
                    else
                    {
                        var catMapped = _mapper.Map<CategoryMenu>(entity);
                        _context.CategoryMenus.Add(catMapped);
                        _context.SaveChanges();

                        response.Succeeded = true;
                        response.Data = "Success";
                        return response;
                    }
                }
                else
                    response.Succeeded = false;
                    response.Data = "Failure";
                    return response;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<DataResponse<string>> UpdateCategoryMenu(CategoryMenuDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                //CategoryMenu categoryMenu = Extentions.AutoMapperProfile.MapForRegisterCategory(model);
                var catMapped = _mapper.Map<CategoryMenu>(model);
                _cat.Update(catMapped);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                //if (entity != null)
                //{
                    response.Succeeded = true;
                    response.Data = "Success";
                //}
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UpdateEmployee: Update customer failed when trying to save in Portal db {e.Message}");
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
    }
}


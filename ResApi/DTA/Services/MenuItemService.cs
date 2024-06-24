using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
	public class MenuItemService : BaseService<MenuItem>, IMenuItem
    {
		private readonly ILogger<MenuItem> _logger;
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		public MenuItemService(DataContext context,ILogger<MenuItem> logger,IMapper mapper) :base(context)
		{
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}

		public async Task<DataResponse<string>> Add(MenuItemDTO entity)
		{
			try
			{
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };
				if(entity != null)
				{
					var itemExists = await _context.MenuItems.AnyAsync(x => x.Name == entity.Name);
					if (itemExists)
					{
                        response.Succeeded = false;
                        response.Data = "Exists";
                        return response;
					}
					else
					{
						var itemMapped = _mapper.Map<MenuItem>(entity);
						_context.MenuItems.Add(itemMapped);
						_context.SaveChanges();

                        response.Succeeded = true;
                        response.Data = "Success";
                        return response;
                    }
				}
				else
				{
                    response.Succeeded = false;
                    response.Data = "Failure";
                    return response;
                }

            }
            catch 
			{
				throw;
			}
		}


		public async Task<List<MenuItemDTO>> GetAllMenuItems(CancellationToken cancellationToken)
		{
			try
			{
                var entity = await _context.MenuItems
                           .Include(x => x.Category)
                           .Select(x => new MenuItemDTO()
                           {
                               Id = x.Id,
                               CategoryName = x.Category.CategoryName,
                               CategoryId = x.Category.Id,
                               Description = x.Description,
                               Name = x.Name,
                               Price = (decimal)x.Price,
                           })
                           .ToListAsync(cancellationToken);

                var menuItems = _mapper.Map<List<MenuItemDTO>>(entity);

                return menuItems;
            }
			catch 
			{
				throw;
			}

		}
		public async Task<List<MenuItemDTO>> GetAllMenuItemsByCategory(int CategoryId)
		{
			try
			{
                var entity = await _context.MenuItems
                                       .Include(x => x.Category)
                                       .Where(x => x.CategoryId == CategoryId)
                                       .Select(x => new MenuItemDTO()
                                       {
                                           Id = x.Id,
                                           CategoryName = x.Category.CategoryName,
                                           CategoryId = x.Category.Id,
                                           Description = x.Description,
                                           Name = x.Name,
                                           Price = (decimal)x.Price,
                                       }).ToListAsync();
                return entity;
            }
			catch 
			{
				throw;
			}
			
		}

        public async Task<DataResponse<string>> Register(MenuItemDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfUserExists = await _context.MenuItems
                       .AnyAsync(x => x.Name == model.Name);
            if (checkIfUserExists)
            {
                response.ErrorMessage = "Perdoruesi me usernamin: " + model.Name + " ekziston";
                return response;
            }

            try
            {
                MenuItem menuItem = Extentions.AutoMapperProfile.MapForRegisterMenu(model);
                _iMenuItem.Add(menuItem);
                // Adding the user to context of users.
                if (menuItem != null)
                {
                    response.Succeeded = true;
                    response.Data = "Error";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin.";
                RequestLogger.WriteResAPIRequests("HTTP POST Response BuyOffer: ", response);
                //ILogger.Error(e, $"CreateCustomerProfile: On adding user in customer portal db error {e.Message}");
            }
            return response;
        }

        public async Task<DataResponse<string>> UpdateMenuItem(MenuItemDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                MenuItem employee = Extentions.AutoMapperProfile.MapForRegisterMenu(model);
                _iMenuItem.Update(employee);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                //if (entity != null)
                //{
                //    response.Succeeded = true;
                //    response.Data = true;
                //}
            }
            catch (Exception e)
            {
                RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateMenuItem: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
    }
}


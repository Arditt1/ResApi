using System;
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

        public async Task<DataResponse<string>> CreateMenuItem(MenuItemDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfMenuItemExists = await _context.MenuItems
                       .AnyAsync(x => x.Name == model.Name);
            if (checkIfMenuItemExists)
            {
                response.ErrorMessage = "Menyja me emrin: " + model.Name + " ekziston";
                return response;
            }

            try
            {
                var menuitemMapp = _mapper.Map<MenuItem>(model);
                _context.MenuItems.Add(menuitemMapp);
                _context.SaveChanges();
                // Adding the user to context of users.
                if (menuitemMapp != null)
                {
                    response.Succeeded = true;
                    response.Data = "Success";
                    return response;
                }
                else
                {
                    response.Succeeded = false;
                    response.Data = "Failure";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin.";
                RequestLogger.WriteResAPIRequests("HTTP POST Response BuyOffer: ", response);
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
                var employeeMapp = _mapper.Map<MenuItem>(model);
                _context.MenuItems.Update(employeeMapp);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                if (employeeMapp != null)
                {
                    response.Succeeded = true;
                    response.Data = "Success";
                }
                else
                {
                    response.Succeeded = false;
                    response.Data = "Error";
                }
            }
            catch (Exception e)
            {
                //RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateMenuItem: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
    }
}


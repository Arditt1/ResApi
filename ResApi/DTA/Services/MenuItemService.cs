using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class MenuItemService : BaseService<MenuItem>, IMenuItem
    {
		private readonly IMenuItem _iMenuItem;
		private readonly DataContext _context;
		public MenuItemService(DataContext context) :base(context)
		{
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


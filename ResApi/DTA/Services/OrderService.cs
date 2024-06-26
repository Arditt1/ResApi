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
	public class OrderService : BaseService<Order>, IOrder
    {
        private readonly ILogger<Order> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OrderService(DataContext context, ILogger<Order> logger, IMapper mapper) : base(context)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;

        }

        public async Task<DataResponse<string>> CreateOrder(OrderDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfOrderExists = await _context.Orders
                       .AnyAsync(x => x.Id == model.Id);
            if (checkIfOrderExists)
            {
                response.ErrorMessage = "Porosia me id: " + model.Id + " ekziston";
                return response;
            }

            try
            {
                var OrderMapp = _mapper.Map<Order>(model);
                _context.Orders.Add(OrderMapp);
                _context.SaveChanges();
                // Adding the user to context of users.
                if (OrderMapp != null)
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
                RequestLogger.WriteResAPIRequests("HTTP POST Response CreateOrder: ", response);
            }
            return response;
        }

        public async Task<DataResponse<string>> UpdateOrder(OrderDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                var OrderMapp = _mapper.Map<Order>(model);
                _context.Orders.Update(OrderMapp);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                if (OrderMapp != null)
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
                RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateOrder: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
	}
}


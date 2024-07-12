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
using ResApi.DTO.OrderDetail;
using ResApi.Models;

namespace ResApi.DTA.Services
{
    public class OrderDetailService : BaseService<OrderDetail>,IOrderDetail
    {
		private readonly ILogger<OrderDetail> _logger;
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		public OrderDetailService(DataContext context,ILogger<OrderDetail> logger,IMapper mapper) : base(context)
		{
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}
		public async Task<List<GetAllOrderDetailsDTO>> GetAllOrderDetails(CancellationToken cancellationToken)
		{
			var entity = await _context.OrderDetails
									   .Include(x => x.Order)
									   .Include(x => x.MenuItem)
									   .Include(x => x.Order.Waiter)
									   .Select(x => new GetAllOrderDetailsDTO()
									   {
										 CategoryId = (int)x.MenuItem.CategoryId,
										 WaiterId = x.Order.WaiterId,
										 MenuItemId = x.MenuItemId,
										 OrderId = x.OrderId,
										 Price = x.Price,
										 TableId = x.Order.TableId,
										 Id = x.Id,

									   })
									   .ToListAsync(cancellationToken);

			var orderDetails = _mapper.Map<List<GetAllOrderDetailsDTO>>(entity);
			return orderDetails;
		}


        public async Task<DataResponse<string>> CreateOrderDetail(OrderDetailDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            try
            {
                var orderdetailMapp = _mapper.Map<OrderDetail>(model);
                _context.OrderDetails.Add(orderdetailMapp);
                _context.SaveChanges();
                // Adding the orderdetailMapp to context.
                if (orderdetailMapp != null)
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
                //RequestLogger.WriteResAPIRequests("HTTP POST Response BuyOffer: ", response);
            }
            return response;
        }

        public async Task<DataResponse<string>> UpdateOrderDetail(OrderDetailDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                var orderdetailMapp = _mapper.Map<OrderDetail>(model);
                _context.OrderDetails.Update(orderdetailMapp);
                if (orderdetailMapp != null)
                {
                    response.Succeeded = true;
                    response.Data = "Success";
                }
                else
                {
                    response.Data = "Error";
                    response.Succeeded = false;
                }    
            }
            catch (Exception e)
            {
                //RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateOrderDetail: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }



    }
}


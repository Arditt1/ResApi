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
            try
            {
                var entity = await _context.OrderDetails
                           .Include(x => x.Order).ThenInclude(x => x.Waiter)
                           .Include(x => x.MenuItem)
                           .Select(x => new GetAllOrderDetailsDTO()
                           {
                               CategoryId = (int)(x.MenuItem.CategoryId ?? default(int)),
                               WaiterId = (int)(x.Order.WaiterId ?? default(int)),
                               MenuItemId = (int)(x.MenuItemId ?? default(int)),
                               OrderId = (int)(x.OrderId ?? default(int)),
                               TotalPrice = (decimal)(x.TotalPrice ?? default(decimal)),
                               TableId = (int)(x.Order.TableId ?? default(int)) ,
                               Id = x.Id,
                               CategoryName = x.MenuItem.Category.CategoryName ?? string.Empty,
                               MenuItemName = x.MenuItem.Name ?? string.Empty,
                               TableNr = x.Order.Table.TableNumber,
                               WaiterUsername = x.Order.Waiter.Name ?? string.Empty,
                               OrderPrice = x.OrderPrice,
                               MenuItems = x.Order.OrderDetails.Select(od => new MenuItemDTO
                               {
                                   Id = od.MenuItem.Id,
                                   Name = od.MenuItem.Name ?? string.Empty,
                                   Price = od.MenuItem.Price ?? default(decimal),
                                   CategoryId = (int?)(od.MenuItem.CategoryId ?? default(int)),

                               }).ToList(),
                           })
                           .ToListAsync(cancellationToken);

                var orderDetails = _mapper.Map<List<GetAllOrderDetailsDTO>>(entity);
                return orderDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                throw;
            }

		}

        public async Task<DataResponse<string>> OrderFood (List<OrderFoodDTO> props,int? tableId, int? waiterId, decimal totalPrice, CancellationToken cancellationToken)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            try
            { 

                var orders = new Order()
                {
                     TableId = tableId,
                     WaiterId = waiterId,
                     OrderTime = DateTime.Now,
                };
                _context.Orders.Add(orders);
                await _context.SaveChangesAsync(cancellationToken);

                var latestOrderInserted = await _context.Orders.OrderByDescending(x => x.OrderTime).Select(x=>x.Id).FirstOrDefaultAsync(cancellationToken);

                var orderDetails = props.Select(item => new OrderDetail
                {
                    OrderId = latestOrderInserted,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    OrderPrice = item.OrderPrice,
                    TotalPrice = item.OrderPrice * item.Quantity,

                }).ToList();

                _context.OrderDetails.AddRange(orderDetails);

                await _context.SaveChangesAsync(cancellationToken);

                response.Data = "Porosia u shtua me sukses!";
                response.Succeeded = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                response.Data = ex.Message;
                response.Succeeded = false;
            }
            return response;

        }

        public async Task<GetAllOrderDetailsDTO> GetOneOrderDetail(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.OrderDetails
                            .Where(x=>x.OrderId == orderId)
                           .Include(x => x.Order)
                           .Include(x => x.MenuItem)
                           .Include(x => x.Order.Waiter)
                           .Select(x => new GetAllOrderDetailsDTO()
                           {
                               OrderId = x.OrderId,
                               TotalPrice = x.TotalPrice,
                               CategoryName = x.MenuItem.Category.CategoryName,
                               TableNr = x.Order.Table.TableNumber,
                               WaiterUsername = x.Order.Waiter.Name,
                               MenuItems = x.Order.OrderDetails.Select(od => new MenuItemDTO
                               {
                                   Id= od.MenuItem.Id,
                                   Name = od.MenuItem.Name,
                                   Price = od.MenuItem.Price
                               }).ToList(),
                           }).FirstOrDefaultAsync(cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                throw;
            }
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


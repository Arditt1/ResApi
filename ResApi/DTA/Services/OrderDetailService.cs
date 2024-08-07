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
using ResApi.DTO.LoginDTO;
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
        public async Task<List<GetAllOrderDetailsDTO>> GetAllOrderDetails(UserDTO user, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new List<GetAllOrderDetailsDTO>();

                if(user.Username.ToLower() != "admin")
                {
                    entity = await _context.Orders
                   .Where(o => o.Status != "Completed" && o.WaiterId == user.Id)
                   .Include(o => o.Table) // Eagerly load Table entity
                   .Include(o => o.OrderDetails)
                       .ThenInclude(od => od.MenuItem) // Eagerly load MenuItem entity
                   .Select(o => new GetAllOrderDetailsDTO()
                   {
                       Id = o.Id,
                       OrderId = o.Id,
                       TableNr = o.Table.TableNumber,
                       WaiterUsername = o.Waiter.Name ?? string.Empty,
                       MenuItems = o.OrderDetails.Select(od => new MenuItemDTO
                       {
                           Id = od.MenuItem.Id,
                           Name = od.MenuItem.Name ?? string.Empty,
                           Price = od.MenuItem.Price ?? default(decimal),
                           CategoryId = (int?)(od.MenuItem.CategoryId ?? default(int)),
                           Quantity = od.Quantity,
                           OrderPrice = od.MenuItem.Price * od.Quantity,
                       }).ToList(),
                       TotalPrice = o.OrderDetails.Sum(od => od.Quantity * od.MenuItem.Price),
                       Quantity = (int)o.OrderDetails.Sum(od => od.Quantity),
                       Status = o.Status,
                       OrderTime = o.OrderTime,
                   })
                   .OrderBy(dto => dto.Status != "New")
                   .ThenByDescending(dto => dto.OrderTime)
                   .ToListAsync(cancellationToken);
                }
                else
                {
                    entity = await _context.Orders
                   .Where(o => o.Status != "Completed")
                   .Include(o => o.Table) // Eagerly load Table entity
                   .Include(o => o.OrderDetails)
                       .ThenInclude(od => od.MenuItem) // Eagerly load MenuItem entity
                   .Select(o => new GetAllOrderDetailsDTO()
                   {
                       Id = o.Id,
                       OrderId = o.Id,
                       TableNr = o.Table.TableNumber,
                       WaiterUsername = o.Waiter.Name ?? string.Empty,
                       MenuItems = o.OrderDetails.Select(od => new MenuItemDTO
                       {
                           Id = od.MenuItem.Id,
                           Name = od.MenuItem.Name ?? string.Empty,
                           Price = od.MenuItem.Price ?? default(decimal),
                           CategoryId = (int?)(od.MenuItem.CategoryId ?? default(int)),
                           Quantity = od.Quantity,
                           OrderPrice = od.MenuItem.Price * od.Quantity,
                       }).ToList(),
                       TotalPrice = o.OrderDetails.Sum(od => od.Quantity * od.MenuItem.Price),
                       Quantity = (int)o.OrderDetails.Sum(od => od.Quantity),
                       Status = o.Status,
                       OrderTime = o.OrderTime,
                   })
                   .OrderBy(dto => dto.Status != "New")
                   .ThenByDescending(dto => dto.OrderTime)
                   .ToListAsync(cancellationToken);
                }

                


                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                throw;
            }
        }

        public async Task<DataResponse<string>> ChangeOrdersStatus(int orderId,string statusName)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };
            try
            {
                var myOrderId = _context.OrderDetails.Where(x => x.Id == orderId).Select(x=>x.OrderId).FirstOrDefault();
                var myOrder = _context.Orders.Where(x => x.Id == myOrderId).FirstOrDefault();
                myOrder.Status = statusName;
                _context.Orders.Update(myOrder);
                await _context.SaveChangesAsync();

                response.Data = "Statusi u ndryshua me sukses!";
                response.Succeeded = true;


            }catch(Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                response.Data = ex.Message;
                response.Succeeded = false;
            }
            return response;
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
                     Status = "New",
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

                }).ToList();

                
                orderDetails.ForEach(item => item.TotalPrice = item.OrderPrice * item.Quantity);

                _context.OrderDetails.AddRange(orderDetails);

                await _context.SaveChangesAsync(cancellationToken);

                response.Data = "Porosia me numer #"+ latestOrderInserted +" u shtua me sukses!";
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
                               Status = x.Order.Status,
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
                await _context.SaveChangesAsync();
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


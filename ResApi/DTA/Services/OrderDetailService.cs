using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
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


    }
}


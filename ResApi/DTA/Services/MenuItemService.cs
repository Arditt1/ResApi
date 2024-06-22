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
			var entity = await _context.MenuItems
									   .Include(x => x.Category)
									   .Select(x=> new MenuItemDTO()
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

	}
}


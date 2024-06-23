using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO.Tables;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class TableService : BaseService<Table> , ITable
    {
		private readonly ILogger<Table> _logger;
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		public TableService(DataContext context,ILogger<Table> logger,IMapper mapper) : base(context)
		{
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<TableDTO>> FreeTables()
		{
			try
			{
                var occupiedTableIds = await _context.TableWaiters.Select(tw => tw.TableId).Distinct().ToListAsync();

                var entity = await _context.Tables
										   .Where(x => !occupiedTableIds.Contains(x.Id))
										   .ToListAsync();
				var mapped = _mapper.Map<List<TableDTO>>(entity);

				return mapped;
			}
			catch
			{
				throw;
			}
		}


		public async Task<DataResponse<string>> Register(TableDTO entity)
		{
			try
			{
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };
				if(entity != null)
				{
					var tableExists = await _context.Tables.AnyAsync(x => x.TableNumber == entity.TableNumber);
					if (tableExists)
					{
                        response.Succeeded = false;
                        response.Data = "Exists";
                        return response;
					}
					else
					{
						var tableMapped = _mapper.Map<Table>(entity);
						_context.Tables.Add(tableMapped);
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
	}
}


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
using ResApi.DTO;
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
		public async Task<List<TableDTO>> GetAllTAbles(int waiterId,string role)
		{
			var entity = new List<TableDTO>();

			if(role.ToLower() == "admin")
			{
                entity = await _context.TableWaiters.Include(x => x.Table)
                                                 .Select(x => new TableDTO()
                                                 {
                                                     Id = x.Id,
                                                     TableNumber = x.Table.TableNumber,
                                                 }).ToListAsync();
			}
			else
			{
                entity = await _context.TableWaiters.Include(x => x.Table).Where(x => x.WaiterId == waiterId)
                                 .Select(x => new TableDTO()
                                 {
                                     Id = x.Id,
                                     TableNumber = x.Table.TableNumber,
                                 }).ToListAsync();
            }

			return entity;
		}
		public async Task<EmployeeDTO> WaiterInfo(int tableId)
		{
			try
			{
				var entity = await _context.TableWaiters.Include(x => x.Waiter)
												  .Where(x => x.TableId == tableId)
												  .Select(x => new EmployeeDTO()
												  {
													 Id = x.Waiter.Id,
													 Username= x.Waiter.Username,
													 ContactInfo = x.Waiter.ContactInfo,
													 TableId = x.TableId,
												  }).FirstOrDefaultAsync();
				return entity;

			}
			catch (Exception ex)
			{
				Console.WriteLine("Error=", ex.Message);
				throw;
			}
		}


		public async Task<DataResponse<string>> Register(int tableNumber)
		{
			try
			{
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };
				if(tableNumber > 0)
				{
					var tableExists = await _context.Tables.AnyAsync(x => x.TableNumber == tableNumber);
					if (tableExists)
					{
                        response.Succeeded = false;
                        response.Data = "Exists";
                        return response;
					}
					else
					{
						Table tbl = new()
						{
							TableNumber = tableNumber,
							Seats = 5,
						};


						var tableMapped = _mapper.Map<Table>(tbl);
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


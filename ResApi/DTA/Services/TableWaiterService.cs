using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO.Tables;
using ResApi.DTO.TableWaiter;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class TableWaiterService : BaseService<TableWaiter>, ITableWaiter
    {
        private readonly ILogger<TableWaiter> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TableWaiterService(DataContext context,ILogger<TableWaiter> logger, IMapper mapper) : base(context)
		{
            _context = context;
            _logger = logger;
            _mapper = mapper;
		}

        public async Task<List<TableDTO>> MyTables(int waiterId)
        {
            try
            {
                var entity = await _context.TableWaiters.Include(x=>x.Table).Where(x => x.WaiterId == waiterId).ToListAsync();
                var mapped = _mapper.Map<List<TableDTO>>(entity);
                return mapped;
            }
            catch
            {
                throw;
            }


        }

        public async Task<DataResponse<string>> Register(TableWaiterDTO entity)
        {
            try
            {
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };
                if (entity != null)
                {
                    var itemExists = await _context.TableWaiters.AnyAsync(x => x.WaiterId == entity.WaiterId && x.TableId == entity.TableId);
                    if (itemExists)
                    {
                        response.Succeeded = false;
                        response.Data = "Exists";
                        return response;
                    }
                    else
                    {
                        var itemMapped = _mapper.Map<TableWaiter>(entity);
                        _context.TableWaiters.Add(itemMapped);
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


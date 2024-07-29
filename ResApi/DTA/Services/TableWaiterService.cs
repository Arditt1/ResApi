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
        public async Task<DataResponse<string>> RemoveWaiterFromTable(int tableNumber, int waiterId)
        {
            try
            {
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

                var entity = await _context.TableWaiters.Include(x => x.Table).Where(x => x.WaiterId == waiterId && x.Table.TableNumber == tableNumber).FirstOrDefaultAsync();
                
                if(entity == null)
                {
                    throw new Exception("Table not found!");
                }
                else
                {
                    _context.TableWaiters.Remove(entity);
                    await _context.SaveChangesAsync();

                    response.Data = "true";
                    response.Succeeded = true;
                }

                return response;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error=", ex.Message);
                throw;
            }
        }

        public async Task<DataResponse<string>> AssignTableToWaiter(int tableNumber,int waiterId)
        {
            try
            {
                var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty  };

                var entity = await _context.TableWaiters.Include(x => x.Table).Where(x => x.WaiterId == waiterId && x.Table.TableNumber == tableNumber).ToListAsync();
                if(entity.Any(x=>x.Table.TableNumber == tableNumber))
                {
                    throw new Exception("Table already assigned!");
                }

                var tableId = _context.Tables.Where(x => x.TableNumber == tableNumber).Select(x=>x.Id).First();

                TableWaiter twTable = new()
                {
                 TableId = tableId,
                 WaiterId = waiterId,
                };
                _context.TableWaiters.Add(twTable);
                await _context.SaveChangesAsync();

                response.Succeeded = true;
                response.Data = "true";

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine("error=", ex.Message);
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

        public async Task<DataResponse<string>> UpdateTableWaiter(TableWaiterDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                var TableWaiterMapp = _mapper.Map<TableWaiter>(model);
                _context.TableWaiters.Update(TableWaiterMapp);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                if (TableWaiterMapp != null)
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
                //RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateTableWaiter: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
    }
}


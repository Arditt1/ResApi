using System;
using ResApi.DataResponse;
using System.Threading.Tasks;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO.Tables;
using ResApi.Models;
using System.Collections.Generic;
using ResApi.DTO;

namespace ResApi.DTA.Intefaces
{
	public interface ITable : IBaseService<Table>
    {
        Task<DataResponse<string>> Register(int tableNumber);
        Task<List<TableDTO>> FreeTables();
        Task<EmployeeDTO> WaiterInfo(int tableId);
        Task<List<TableDTO>> GetAllTAbles(int waiterId,string username);
    }
}


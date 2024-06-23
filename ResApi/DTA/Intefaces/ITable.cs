using System;
using ResApi.DataResponse;
using System.Threading.Tasks;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO.Tables;
using ResApi.Models;
using System.Collections.Generic;

namespace ResApi.DTA.Intefaces
{
	public interface ITable : IBaseService<Table>
    {
        Task<DataResponse<string>> Register(TableDTO entity);
        Task<List<TableDTO>> FreeTables();

    }
}


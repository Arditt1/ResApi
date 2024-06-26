using System;
using ResApi.DataResponse;
using System.Threading.Tasks;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO.TableWaiter;
using ResApi.Models;
using System.Collections.Generic;
using ResApi.DTO.Tables;

namespace ResApi.DTA.Intefaces
{
	public interface ITableWaiter : IBaseService<TableWaiter>
    {
        Task<DataResponse<string>> Register(TableWaiterDTO entity);
        Task<List<TableDTO>> MyTables(int waiterId);
        Task<DataResponse<string>> UpdateTableWaiter(TableWaiterDTO model);
    }
}


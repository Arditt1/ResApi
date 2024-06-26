using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
    public interface IOrder : IBaseService<Order>
    {
        Task<DataResponse<string>> CreateOrder(OrderDTO model);
        Task<DataResponse<string>> UpdateOrder(OrderDTO model);
    }
}


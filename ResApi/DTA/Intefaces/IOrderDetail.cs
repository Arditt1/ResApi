using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.DTO.OrderDetail;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
    public interface IOrderDetail : IBaseService<OrderDetail>
    {
        Task<List<GetAllOrderDetailsDTO>> GetAllOrderDetails(CancellationToken cancellationToken);
        Task<DataResponse<string>> CreateOrderDetail(OrderDetailDTO model);
        Task<DataResponse<string>> UpdateOrderDetail(OrderDetailDTO model);
        Task<GetAllOrderDetailsDTO> GetOneOrderDetail(int orderId, CancellationToken cancellationToken);
        Task<DataResponse<string>> OrderFood(List<OrderFoodDTO> props, int? tableId, int? waiterId, CancellationToken cancellationToken);
    }
}


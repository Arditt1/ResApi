using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.DTO.LoginDTO;
using ResApi.DTO.OrderDetail;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
    public interface IOrderDetail : IBaseService<OrderDetail>
    {
        Task<List<GetAllOrderDetailsDTO>> GetAllOrderDetails(UserDTO user,CancellationToken cancellationToken);
        Task<DataResponse<string>> CreateOrderDetail(OrderDetailDTO model);
        Task<DataResponse<string>> UpdateOrderDetail(OrderDetailDTO model);
        Task<GetAllOrderDetailsDTO> GetOneOrderDetail(int orderId, CancellationToken cancellationToken);
        Task<DataResponse<string>> OrderFood(List<OrderFoodDTO> props, int? tableId, int? waiterId, decimal totalPrice, CancellationToken cancellationToken);
        Task<DataResponse<string>> ChangeOrdersStatus(int orderId, string statusName);
    }
}


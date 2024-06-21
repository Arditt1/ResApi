using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO.OrderDetail;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
    public interface IOrderDetail : IBaseService<OrderDetail>
    {
        Task<List<GetAllOrderDetailsDTO>> GetAllOrderDetails(CancellationToken cancellationToken);

    }
}


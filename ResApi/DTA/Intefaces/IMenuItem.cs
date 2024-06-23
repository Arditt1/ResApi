using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Intefaces
{
    public interface IMenuItem : IBaseService<MenuItem>
    {
        Task<List<MenuItemDTO>> GetAllMenuItems(CancellationToken cancellationToken);
        Task<DataResponse<string>> Add(MenuItemDTO entity);
        Task<List<MenuItemDTO>> GetAllMenuItemsByCategory(int CategoryId);
    }
}


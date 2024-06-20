using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
    public interface IMenuItem : IBaseService<MenuItem>
    {
        Task<DataResponse<string>> Register(MenuItemDTO model);

        Task<DataResponse<string>> UpdateMenuItem(MenuItemDTO model);
    }
}


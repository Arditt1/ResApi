using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
    public interface IPermission : IBaseService<Permission>
    {
        Task<DataResponse<string>> CreatePermission(PermissionDTO model);
        Task<DataResponse<string>> UpdatePermission(PermissionDTO model);
    }
}


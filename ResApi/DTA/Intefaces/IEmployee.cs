﻿using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
    public interface IEmployee : IBaseService<Employee>
    {
        Task<DataResponse<EmployeeDTO>> Authenticate(EmployeeDTO model);
        Task<DataResponse<string>> Register(EmployeeDTO model);
        Task<DataResponse<string>> UpdateEmployee(EmployeeDTO model);
        Task<DataResponse<bool>> ChangePassword(ChangePasswordDto model, CancellationToken cancellationToken);//DisableEmployee
        Task<DataResponse<bool>> DisableEmployee(DisableEmployeeDTO model, CancellationToken cancellationToken);
    }
}


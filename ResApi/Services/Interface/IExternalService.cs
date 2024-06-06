using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.Models;
using ResApi.Dtos;
// ReSharper disable InconsistentNaming

namespace ResApi.Services.Interface
{
    public interface IExternalService
    {
        Task<DataResponse<Guid>> CreateCustomerProfile(RegisterUserDto customer);
        Task<DataResponse<ExtCustomerProfile>> GetCustomerProfile(Guid externalCustomerId);
        Task<DataResponse<bool>> UpdateCustomer(CustomerDto customer);
        Task<DataResponse<bool>> ValidateCustomerProfile(RegisterUserDto customer);
        Task<DataResponse<bool>> SendMail(ExtMailData data);

    }
}

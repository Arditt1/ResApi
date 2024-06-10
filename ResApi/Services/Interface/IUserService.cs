using ResApi.DataResponse;
using ResApi.Models;
using ResApi.Dtos;
using System;
using System.Threading.Tasks;

namespace ResApi.Services.Interface
{
    public interface IUserService
    {
        Task<DataResponse<AuthenticateUserDto>> Authenticate(AuthenticateUserDto model);
        Task<DataResponse<string>> Register(RegisterUserDto model);
        DataResponse<Employee> GetEmployeeProfile(int id);
        Task<DataResponse<bool>> UpdateEmployee(EmployeeDto model);
        Task<DataResponse<bool>> ChangePassword(ChangePasswordDto model);
        Task<DataResponse<AuthenticateUserDto>> Login(string username); 
    }
}
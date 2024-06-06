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
        Task<DefOperator> GetById(int id);
        Task<DataResponse<bool>> RequestPassword(RequestPasswordDto model);
        Task<DataResponse<bool>> UpdateCustomer(CustomerDto model);
        Task<DataResponse<bool>> ChangePassword(ChangePasswordDto model);
        //Task<DataResponse<ExtCustomerProfile>> GetCustomer(Guid id);
        Task<DataResponse<bool>> ConfirmEmail(string token);
        Task<DataResponse<bool>> ResendEmailConfirmation(string email);
        //Task<DataResponse<CheckProfileDto>> HasProfile(string email);
        Task<DataResponse<bool>> ConfirmVerificationCode(string verificationCode, string email);
        Task<DataResponse<AuthenticateUserDto>> Login(string email); 
        Task<DataResponse<bool>> ResendVerificationCode(string email);
    }
}
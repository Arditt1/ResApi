using ResApi.Models;
using ResApi.DataResponse;
using ResApi.Dtos;
using System.Threading.Tasks;

namespace ResApi.Services.Interface
{
    public interface IUserRepository : IRepository<Employee>
    {
        Task<DataResponse<Employee>> Authenticate(AuthenticateUserDto authenticateRequest);
        Task<DataResponse<bool>> ChangePassword(string Username, string newPassword);
        Task<bool> CheckUserExists(string Username);

        //Task<bool> GetByUsername(string username);
        Task<DataResponse<Employee>> Login(string username);
        Task<bool> DeleteEmployee(int Username);
    }
}

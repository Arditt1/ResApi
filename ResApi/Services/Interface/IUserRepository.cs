using ResApi.Models;
using ResApi.DataResponse;
using ResApi.Dtos;
using System.Threading.Tasks;

namespace ResApi.Services.Interface
{
    public interface IUserRepository : IRepository<DefOperator>
    {
        Task<DataResponse<DefOperator>> Authenticate(AuthenticateUserDto authenticateRequest, bool isEmailConfirmationMode = false);
        Task<DataResponse<bool>> ChangePassword(string email, string newPassword);
        Task<bool> CheckUserExists(string email);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
        Task<DataResponse<DefOperator>> Login(string email);
    }
}

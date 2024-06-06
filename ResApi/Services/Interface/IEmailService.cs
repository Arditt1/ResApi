using ResApi.DataResponse;
using System.Threading.Tasks;

namespace ResApi.Services.Interface
{
    public interface IEmailService
    {
        Task<DataResponse<bool>> SendForgottenPasswordEmail(string email, string newPassword);

    }
}
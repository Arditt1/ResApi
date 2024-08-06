using ResApi.DTO.LoginDTO;

namespace ResApi.DTA.Intefaces
{
    public interface IAuth
	{
        UserDTO AuthenticateUser(UserLoginDTO userLogin);

    }
}


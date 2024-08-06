using System;
using System.Linq;
using ResApi.DTA.Intefaces;
using ResApi.DTO.LoginDTO;
using ResApi.Models;

namespace ResApi.DTA.Services
{
    public class AuthService : IAuth
	{
        private readonly DataContext _context;
        public AuthService(DataContext context) 
        {
            _context = context;
        }
        public UserDTO AuthenticateUser(UserLoginDTO userLogin)
        {
            try
            {
                var user = _context.Employees.Where(x => x.Username == userLogin.Username && x.Password == userLogin.Password).FirstOrDefault();
                if (user != null)
                {
                    var roleName = _context.Roles.Where(x => x.Id == user.RoleId).Select(x => x.RoleName).FirstOrDefault();
                    var userDto = new UserDTO()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Password = user.Password,
                        Username = user.Username,
                        RoleName = roleName,
                        Surname = user.Surname,

                    };
                    return userDto;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex=", ex.Message);
                throw;
            }
        }
    }
}


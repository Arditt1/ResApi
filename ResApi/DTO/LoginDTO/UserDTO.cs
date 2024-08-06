using System;
namespace ResApi.DTO.LoginDTO
{
	public class UserDTO
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RoleName { get; set; }
    }
}


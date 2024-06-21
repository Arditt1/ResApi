using System;
namespace ResApi.DTO
{
	public class EmployeeDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? Status { get; set; }
        public string? ContactInfo { get; set; }
    }
}


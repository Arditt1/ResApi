using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class AuthenticateUserDto
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactInfo { get; set; }
        public List<Order> Orders { get; set; }
        public List<TableWaiter> AssignedTables { get; set; }
        public bool Status { get; set; }
        public string Token { get; set; }

        public AuthenticateUserDto(int employeeID, string name, string surname, int roleID, Role role, string username, string password, string contactInfo, List<Order> orders, List<TableWaiter> assignedTables, bool status, string token)
        {
            EmployeeID = employeeID;
            Name = name;
            Surname = surname;
            RoleID = roleID;
            Role = role;
            Username = username;
            Password = password;
            ContactInfo = contactInfo;
            Orders = orders;
            AssignedTables = assignedTables;
            Status = status;
            Token = token;
        }

        public AuthenticateUserDto()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<Permission_waiter> Permissions { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
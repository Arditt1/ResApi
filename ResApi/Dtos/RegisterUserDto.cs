﻿using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResApi.Dtos
{
    public partial class RegisterUserDto
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactInfo { get; set; }

        // You may need to change the types of OrdersDto and TableWaitersDto based on your requirements
        public List<Order> Orders { get; set; }
        public List<TableWaiter> AssignedTables { get; set; }

        public bool Status { get; set; }
    }
}

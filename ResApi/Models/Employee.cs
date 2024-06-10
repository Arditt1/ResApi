using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class Employee
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
        public bool NewRecord { get; set; }


        public void Load(int employeeId)
        {
            // Assuming you're using some ORM or direct database access
            // Here's a pseudo-code representation for database access

            using (var dbContext = new RestApiDbContext())
            {
                var employee = dbContext.Employee.FirstOrDefault(e => e.EmployeeID == employeeId);
                if (employee != null)
                {
                    // Populate properties from database record
                    EmployeeID = employee.EmployeeID;
                    Name = employee.Name;
                    Surname = employee.Surname;
                    RoleID = employee.RoleID;
                    Role = employee.Role; // Assuming Role is a navigation property
                    Username = employee.Username;
                    Password = employee.Password;
                    ContactInfo = employee.ContactInfo;
                    // Assuming Orders and AssignedTables are also navigation properties
                    Orders = employee.Orders.ToList();
                    AssignedTables = employee.AssignedTables.ToList();
                    Status = employee.Status;
                }
                else
                {
                    // If employee with given ID doesn't exist, set NewRecord to true
                    NewRecord = true;
                }
            }
        }
    }
}
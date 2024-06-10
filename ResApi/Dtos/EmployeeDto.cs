using ResApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResApi.Dtos
{
    public partial class EmployeeDto
    {
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        public int RoleID { get; set; }

        // You may need to change the type of RoleDto based on your requirements
        public Role Role { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string ContactInfo { get; set; }

        // You may need to change the types of OrdersDto and TableWaitersDto based on your requirements
        public List<Order> Orders { get; set; }
        public List<TableWaiter> AssignedTables { get; set; }

        public bool Status { get; set; }

    }
}

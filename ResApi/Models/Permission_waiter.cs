using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class Permission_waiter
    {
        public int PermissionID { get; set; }
        public string Permission { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
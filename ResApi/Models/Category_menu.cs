using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class CategoryMenu
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Photo { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
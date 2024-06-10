using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class MenuItem
    {
        public int MenuItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public CategoryMenu Category { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
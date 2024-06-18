using ResApi.Models.Shared;
using System.Collections.Generic;

namespace ResApi.DTO
{
    public class MenuItemDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}

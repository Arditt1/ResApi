using ResApi.Models.Shared;
using System.Collections.Generic;

namespace ResApi.Models
{
    public partial class CategoryMenu : BaseEntity
    {
        public CategoryMenu()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public string? CategoryName { get; set; }
        public string? Photo { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}

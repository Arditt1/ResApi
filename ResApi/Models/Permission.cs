using ResApi.Models.Shared;

namespace ResApi.Models
{
    public partial class Permission : BaseEntity
    {
        public string? Permission1 { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}

using ResApi.Models.Shared;

namespace ResApi.Models
{
    public partial class TableWaiter : BaseEntity
    {
        public int? TableId { get; set; }
        public int? WaiterId { get; set; }

        public virtual Table? Table { get; set; }
        public virtual Employee? Waiter { get; set; }
    }
}

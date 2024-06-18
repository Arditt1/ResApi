using ResApi.Models.Shared;
using System;
using System.Collections.Generic;

namespace ResApi.DTO
{
    public class OrderDTO :BaseEntity
    {

        public int? TableId { get; set; }
        public int? WaiterId { get; set; }
        public DateTime? OrderTime { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}

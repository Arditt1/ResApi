using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public class Table
    {
        public int TableID { get; set; }
        public int TableNumber { get; set; }
        public int Seats { get; set; }
        public List<Order> Orders { get; set; }
        public List<TableWaiter> AssignedWaiters { get; set; }
    }
}
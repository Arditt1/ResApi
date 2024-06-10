using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public class TableWaiter
    {
        public int TableWaiterID { get; set; }
        public int TableID { get; set; }
        public Table Table { get; set; }
        public int WaiterID { get; set; }
        public Employee Waiter { get; set; }
    }
}
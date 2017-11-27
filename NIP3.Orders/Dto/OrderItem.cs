using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIP3.Orders.Dto
{
    public class OrderItem
    {
        public string ProductCodeName { get; set; }
        public uint Quantity { get; set; }
    }
}
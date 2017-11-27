using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIP3.Orders.Dto
{
    public class OrderStatus
    {
        public string OrderNumber { get; set; }
        public bool IsDelivered { get; set; }
    }
}
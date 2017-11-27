using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIP3.Orders.Dto
{
    public class OrderConfirmation
    {
        public string OrderNumber { get; set; }
        public string ShipmentNumber { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace NIP3.Orders.Dto
{
    public class Order
    {
        public Customer Customer;
        public OrderItem[] Items { get; set; }
    }
}
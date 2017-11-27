using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIP3.Orders.Proxies
{
    public class ShipmentException : Exception
    {
        public ShipmentException(string message) : base(message)
        {

        }
    }
}
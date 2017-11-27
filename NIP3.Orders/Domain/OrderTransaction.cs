using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NIP3.Orders.Dto;

namespace NIP3.Orders.Domain
{
    public class OrderTransaction
    {
        public Guid OrderNumber { get; private set; }
        public Order Order { get; private set; }
        public string ShipmentConfirmationNumber { get; private set; }

        public OrderTransaction(Order order, string shipmentConfirmationNumber)
            :this(Guid.NewGuid(), order, shipmentConfirmationNumber)
        {
        }

        internal OrderTransaction(Guid orderNumber, Order order, string shipmentConfirmationNumber)
        {
            OrderNumber = orderNumber;
            Order = order;
            ShipmentConfirmationNumber = shipmentConfirmationNumber;
        }
    }
}
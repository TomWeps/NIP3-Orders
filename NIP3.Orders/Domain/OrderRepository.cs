using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIP3.Orders.Domain
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Dictionary<Guid, OrderTransaction> dictionary;
        public OrderRepository()
        {
            dictionary = new Dictionary<Guid, OrderTransaction>();
        }

        public void Add(OrderTransaction orderTransaction)
        {
            dictionary.Add(orderTransaction.OrderNumber, orderTransaction);
        }    

        public OrderTransaction Get(Guid orderNumber)
        {
            OrderTransaction value;
            dictionary.TryGetValue(orderNumber, out value);
            return value;
        }
    }
}
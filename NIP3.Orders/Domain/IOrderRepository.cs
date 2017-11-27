using System;

namespace NIP3.Orders.Domain
{
    public interface IOrderRepository
    {
        void Add(OrderTransaction orderTransaction);
        OrderTransaction Get(Guid orderNumber);
    }
}
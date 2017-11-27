using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NIP3.Orders.Domain;
using NIP3.Orders.Dto;
using NIP3.Orders.Proxies;
using Swashbuckle.Swagger.Annotations;

namespace NIP3.Orders
{
    [RoutePrefix("api/v1/orders")]
    public class OrderController : ApiController
    {
        private readonly IShipmentProxy shipmentProxy;
        private readonly IOrderRepository orderRepository;
        
        public OrderController(IShipmentProxy shipmentProxy, IOrderRepository orderRepository)
        {
            this.shipmentProxy = shipmentProxy;
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("{orderNumber}", Name = "Get Order Status")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderStatus))]
        public async Task<IHttpActionResult> CheckOrderStatus(string orderNumber)
        {
            var orderTransaction = orderRepository.Get(Guid.Parse(orderNumber));

            if(orderTransaction == null)
            {
                return NotFound();
            }

            bool isDelivered = await shipmentProxy.CheckIfIsDelivered(orderTransaction);

            var result = MapToOrderStatus(orderTransaction, isDelivered);

            return Ok(result);
        }

        [HttpPost]
        [Route("", Name = "Order Product")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderConfirmation))]
        public async Task<IHttpActionResult> CreateOrder(Order order)
        {
            try
            {
                string shipmentNumber = await shipmentProxy.OrderShipmentAsync(order.Customer);

                var orderTransaction = new OrderTransaction(order, shipmentNumber);
                orderRepository.Add(orderTransaction);

                return Ok(MapToOrderConfirmation(orderTransaction));
            }
            catch(ShipmentException)
            {
                return BadRequest();
            }
        }

        private OrderConfirmation MapToOrderConfirmation(OrderTransaction orderTransaction)
        {
            return new OrderConfirmation
            {
                OrderNumber = orderTransaction.OrderNumber.ToString(),
                ShipmentNumber = orderTransaction.ShipmentConfirmationNumber
            };
        }

        private OrderStatus MapToOrderStatus(OrderTransaction orderTransaction, bool isDelivered)
        {
            return new OrderStatus
            {
                OrderNumber = orderTransaction.OrderNumber.ToString(),
                IsDelivered = isDelivered
            };
        }
    }
}
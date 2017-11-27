using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using NIP3.Orders.Dto;
using NIP3.Orders.Test.Helpers;
using NIP3.Orders.Test.Helpers.Pact;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Matchers;
using NIP3.Orders.Domain;

namespace NIP3.Orders.Test
{
    public class OrderControllerTest: IClassFixture<OrdersConsumerPact>
    {
        private string baseUriOrder = "api/v1/orders";
        private string relativeUriShipment = "/api/v1/shipment";
        private readonly IMockProviderService shipmentMockProviderService;
        private readonly OrderServiceTestServer orderServiceTestServer;

        public OrderControllerTest(OrdersConsumerPact consumerShipmentPact)
        {
            shipmentMockProviderService = consumerShipmentPact.ShipmentMockProviderService;
            // NOTE: clears any previously registered interactions before the test is run 
            shipmentMockProviderService.ClearInteractions();

            orderServiceTestServer = new OrderServiceTestServer();
        }

        [Fact]
        public async Task CreateOrder_WithValidData_ShouldReturnConfirmation()
        {
            // Arrange
            
            /* 
            shipmentMockProviderService
                .Given(providerState: "valid shipment data")
                .UponReceiving(description: "a request to shipping order")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = relativeUriShipment,                    
                    Body = new
                    {
                        //NOTE: Note the case sensitivity here, the body will be serialized as per the casing defined.
                        name = Match.Regex(example: "Jan Kowalski", regex: @".+"),
                        address = Match.Regex(example: "Akademicka 16", regex: @".+"),
                        postalCode = Match.Regex(example: "44-100", regex: @".+"),
                        city = Match.Regex(example: "Gliwice", regex: @".+"),
                        countryCode = Match.Regex(example: "PL", regex: @"[A-Z]{2}"),
                        phoneNumber = Match.Regex(example: "32 237 13 10", regex: @".+"),
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" },
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = //TODO
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },                    
                    Body = new 
                    {
                        //NOTE: Note the case sensitivity here, the body will be serialized as per the casing defined.
                        confirmationNumber = Match.Regex(regex: @"XX-\d{1,}", example: "XX-636446338738089968")
                    }                
                }); // Note: Will ResponseWith call must come last as it will register the interaction.
            */

            var order = GetOrderRequestArgs();

            // Act
            OrderConfirmation result =null;
            using (var server = orderServiceTestServer.Create())
            {
                var response = await server.CreateRequest(baseUriOrder)
                    .AddAsJson(order)            
                    .PostAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsAsync<OrderConfirmation>();
                }
            }

            // Asserts

            Assert.NotNull(result);

            // NOTE: Verifies that interactions registered on the mock provider are called once and only once.
            shipmentMockProviderService.VerifyInteractions(); 
        }

        [Fact]
        public async Task CreateOrder_WithInvalidShipmentData_ShouldFail()
        {
            // Arrange
            /*
            shipmentMockProviderService
                .Given(providerState: "invalid shipment data")
                .UponReceiving(description: "a bad request to shipping order")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = relativeUriShipment,                    
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }                       
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = //TODO                   
                });
            */

            var order = GetOrderRequestArgs();

            // Act           
            HttpStatusCode result;
            using (var server = orderServiceTestServer.Create())
            {
                var response = await server.CreateRequest(baseUriOrder)
                    .AddAsJson(order)
                    .PostAsync();

                result = response.StatusCode;
            }

            // Asserts
            Assert.Equal(HttpStatusCode.BadRequest, result);            
            shipmentMockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task GetOrder_WithExistingOrder_ShouldReturnOrderState()
        {
            // Arrange
            string pathRegexPatterm = $"{relativeUriShipment}/\\S+";
            /*
            shipmentMockProviderService
                .Given(providerState: "there is a shipment number 'XX-636446338738089968' not delivered")
                .UponReceiving(description: "a request for shipping order status")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Match.Regex(example: "/api/v1/shipment/XX-636446338738089968", regex: pathRegexPatterm),
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" },                        
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = //TODO
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        isDelivered = false                        
                    }
                });
            */

            string orderNumber = "8946D698-EB6B-43BB-A39D-1C9A89E005F5";

            var orderRepo = orderServiceTestServer.GetOrderRepository();
            orderRepo.Add(
                new OrderTransaction(
                    orderNumber: Guid.Parse(orderNumber),
                    shipmentConfirmationNumber: "XX-636446338738089968",
                    order: GetOrderRequestArgs()
                )
                );

            // Act           
            OrderStatus result=null;
            using (var server = orderServiceTestServer.Create())
            {
                string uri = baseUriOrder + "/" + orderNumber;
                var response = await server.CreateRequest(uri)
                   .GetAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsAsync<OrderStatus>();
                }
            }

            // Asserts
            Assert.NotNull(result);
            Assert.False(result.IsDelivered);
            Assert.Equal("8946d698-eb6b-43bb-a39d-1c9a89e005f5", result.OrderNumber);
            
            shipmentMockProviderService.VerifyInteractions();
        }

        private Order GetOrderRequestArgs()
        {
            Order order = new Order
            {
                Customer = new Customer
                {
                    FirstName = "Jan",                    
                    LastName = "Kowalski",
                    EmailAddress = "jan.kowalski@polsl.pl",
                    PhoneNumber = "32 237 13 10",

                    Address = "Akademicka 16",
                    City = "Gliwice",
                    PostalCode = "44-100",
                    CountryCode = "PL"
                },
                Items = new[]
                {
                    new OrderItem
                    {
                        ProductCodeName = "Cos",
                        Quantity = 1
                    }
                }
            };

            return order;
        }
    }
}

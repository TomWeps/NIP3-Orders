using System;
using System.IO;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace NIP3.Orders.Test.Helpers.Pact
{
    public class OrdersConsumerPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService ShipmentMockProviderService { get; private set; }

        public OrdersConsumerPact()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = TestEnvironment.PactsDirectory,
                LogDir = TestEnvironment.LogsDirectory
            });

            PactBuilder
                .ServiceConsumer(consumerName: "OrdersService")
                .HasPactWith(providerName: "ShipmentService");

            ShipmentMockProviderService = PactBuilder.MockService(TestEnvironment.ShipmentServiceUri.Port);
        }

        public void Dispose()
        {
            PactBuilder.Build(); // NOTE: Will save the pact file once finished
        }
    }
}

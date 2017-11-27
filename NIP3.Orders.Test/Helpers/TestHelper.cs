using Microsoft.Owin.Testing;
using Microsoft.Practices.Unity;
using NIP3.Orders.App_Start;
using NIP3.Orders.Domain;

namespace NIP3.Orders.Test.Helpers
{
    public class OrderServiceTestServer
    {
        private IUnityContainer container;

        public OrderServiceTestServer()
        {
            container = UnityConfig.Init();            
        }
        
        public TestServer Create()
        {
            var testServer = TestServer.Create(app =>
            {
                new Startup().Configuration(app, container);
            });

            return testServer;
        }    
        

        public IOrderRepository GetOrderRepository()
        {
            return container.Resolve<IOrderRepository>();
        }
    }
}

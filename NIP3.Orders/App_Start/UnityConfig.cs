using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using NIP3.Orders.Domain;
using NIP3.Orders.Proxies;
using System.Configuration;

namespace NIP3.Orders.App_Start
{
    public static class UnityConfig
    {
        public static IUnityContainer Init()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }

        internal static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShipmentProxy>(new InjectionFactory((c) =>
            {
                string shipmentServiceUri = ConfigurationManager.AppSettings["ShipmentServiceUri"];
                if (string.IsNullOrWhiteSpace(shipmentServiceUri))
                {
                    throw new ArgumentException("ShipmentServiceUri");
                }

                return new ShipmentProxy(shipmentServiceUri);
            }));

            return container;
        }
    }
}
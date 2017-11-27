using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using NIP3.Orders.App_Start;
using Microsoft.Practices.Unity.WebApi;
using Microsoft.Practices.Unity;

[assembly: OwinStartup(typeof(NIP3.Orders.Startup))]

namespace NIP3.Orders
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration(app, UnityConfig.Init());
        }

        internal void Configuration(IAppBuilder app, IUnityContainer container)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new UnityHierarchicalDependencyResolver(container);

            WebApiConfig.Register(config);
            SwaggerConfig.Register(config);

            app.UseWebApi(config);            
        }
    }
}

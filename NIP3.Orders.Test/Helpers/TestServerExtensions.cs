using System.Net.Http;
using System.Net.Http.Formatting;
using Microsoft.Owin.Testing;

namespace NIP3.Orders.Test.Helpers
{
    public static class TestServerExtensions
    {
        public static RequestBuilder AddAsJson<T>(this RequestBuilder requestBuilder, T value)
        {          
            requestBuilder.AddHeader("Accept", "application/json");
            requestBuilder.And(r => r.Content = new ObjectContent<T>(value, new JsonMediaTypeFormatter()));

            return requestBuilder;
        }
    }
}

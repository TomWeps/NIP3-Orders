using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIP3.Orders.Domain;
using NIP3.Orders.Dto;

namespace NIP3.Orders.Proxies
{
    public class ShipmentProxy : IShipmentProxy
    {
        private const string uriApiRelative = "shipment";
        private readonly HttpClient httpClient;
        private readonly JsonMediaTypeFormatter formatter;

        public ShipmentProxy(string shipmentServiceUri)
        {
            httpClient = ConstructHttpClient(shipmentServiceUri);
            formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
        }

        private HttpClient ConstructHttpClient(string shipmentServiceUri)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(shipmentServiceUri.TrimEnd('/') + "/api/v1/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            

            return client;
        }

        public async Task<string> OrderShipmentAsync(Customer customer)
        {
            throw new NotImplementedException(
                "Implementacja klasy ShipmentProxy nie jest jeszcze w pełni ukończona. " +
                "Punkt 2.Instrukcji zawiera opis jak dokończyć implementację." +
                "Usuń ten wyjątek, gdy metoda GetShipmentResponseData będzie gotowa."
            );
            
            var shipmentRequestData = CreateShipmentRequest(customer);

            try
            {
                HttpResponseMessage response = await httpClient
                    .PostAsync<ShipmentRequest>(uriApiRelative, shipmentRequestData, formatter)
                    .ConfigureAwait(false);                

                var shipmentResponseData = await GetShipmentResponseData(response)
                    .ConfigureAwait(false);

                return shipmentResponseData.ConfirmationNumber;
            }
            catch (HttpRequestException)
            {
                throw new ShipmentException("Shipment service is not available!");
            }
        }

        public async Task<bool> CheckIfIsDelivered(OrderTransaction orderTransaction)
        {
            var requestUri = $"{uriApiRelative}/{orderTransaction.ShipmentConfirmationNumber}";
            var response = await httpClient
                .GetAsync(requestUri)
                 .ConfigureAwait(false);

            var shipmentResponse = await GetShipmentResponseData(response)
                                    .ConfigureAwait(false);

            return shipmentResponse.IsDelivered;
        }

        private ShipmentRequest CreateShipmentRequest(Customer customer)
        {
            var shipmentRequest = new ShipmentRequest
            {
                Name = $"{customer.FirstName} {customer.LastName}",
                PhoneNumber = customer.PhoneNumber,

                Address = customer.Address,
                City = customer.City,
                PostalCode = customer.PostalCode,
                CountryCode = customer.CountryCode,
            };

            return shipmentRequest;
        }

        private async Task<ShipmentResponse> GetShipmentResponseData(HttpResponseMessage response)
        {
            throw new NotImplementedException(
                "Implementacja klasy ShipmentProxy nie jest jeszcze w pełni ukończona. " +
                "Punkt 2.Instrukcji zawiera opis jak dokończyć implementację."
            );

            //TODO
            // - Zweryfikuj HttpStatus odpowiedzi (response) przysłane przez Shipment Service
            // - Jeśli operacja zakończyła się sukcesem, odczytaj przesłane dane(json) 
            //   i zwróć je przy pomocy klasy ShipmentResponse(response.Content.ReadAsAsync<ShipmentResponse>).
            // - W przypadku zasygnalizowania błędu przez ShipmentService(np.gdy dane adresowe są niepoprawne), 
            //   metoda powinna wyrzucić wyjątek ShipmentException.

            // Pomocne linki

            // Opis http statusów: 
            // https://restfulapi.net/http-status-codes/

            // Przykład użycia klasy HttpClient (zapytania REST):
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client


        }
    }
}
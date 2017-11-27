using System.ComponentModel.DataAnnotations;

namespace NIP3.Orders.Proxies
{
    public class ShipmentRequest
    {        
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
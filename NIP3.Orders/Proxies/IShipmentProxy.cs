using System.Threading.Tasks;
using NIP3.Orders.Dto;
using NIP3.Orders.Domain;

namespace NIP3.Orders.Proxies
{
    public interface IShipmentProxy
    {
        Task<string> OrderShipmentAsync(Customer customer);
        Task<bool> CheckIfIsDelivered(OrderTransaction orderTransaction);
    }
}
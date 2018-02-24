using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Orders;
using WebApi.Models.Resources;

namespace WebApi.Models.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Flavour>> GetFlavoursAsync();
        Task<IEnumerable<OrderTotalPriceResource>> PlaceOrder(ShoppingCart shoppingcart, Customer customer);
        //Task<Order> PlaceOrder(ShoppingCart shoppingcart, Customer customer);
        Task<Order> GetOrder(int orderId);
        Task<bool> ConfirmOrder(Order order, ConfirmOrderResource confirmOrder);
    }
}
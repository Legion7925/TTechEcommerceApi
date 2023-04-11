using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Interface
{
    public interface IOrderService
    {
        Task<Order?> AddOrder(Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<Order?> GetOrderById(int orderId);
        IEnumerable<Order> GetOrders(QueryParametersModel queryParameters);
        Task<Order?> UpdateOrder(OrderUpdateModel order, int orderId);
    }

}

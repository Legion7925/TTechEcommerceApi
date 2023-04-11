using AutoMapper;
using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class OrderService : IOrderService
    {
        private readonly EcommerceContext context;
        private readonly IMapper mapper;

        public OrderService(EcommerceContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<Order> GetOrders(QueryParametersModel queryParameters)
        {
            return context.Orders.AsNoTracking().Skip((queryParameters.Page - 1) * queryParameters.Size).Take(queryParameters.Size).ToArray();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await context.Orders.FirstOrDefaultAsync(i => i.Id == orderId);
        }

        public async Task<Order?> AddOrder(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrder(OrderUpdateModel order, int orderId)
        {
            var findOrder = await GetOrderById(orderId);
            if (findOrder != null)
            {
                mapper.Map(order, findOrder);
                await context.SaveChangesAsync();
                return findOrder;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var findOrder = await GetOrderById(orderId);

            if (findOrder == null)
                return false;

            context.Orders.Remove(findOrder);
            await context.SaveChangesAsync();
            return true;
        }
    }
}

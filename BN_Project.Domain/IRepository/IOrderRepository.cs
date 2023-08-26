using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;

namespace BN_Project.Domain.IRepository
{
    public interface IOrderRepository : IGenericRepositroy<Order>
    {
        public Task<List<Order>> GetOrderBoxByStatusWithIncludeOrderDetail(OrderStatus orderStatus, int userId);

        public Task<List<string>> GetProductImagesByOrderId(int orderId);

        public Task<Order> GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(int userId);

        public Task<Order> GetOrderWithIncludeOrderDetail(int orderId);
    }
}

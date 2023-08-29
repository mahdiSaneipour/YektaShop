using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IOrderDetailRepository : IGenericRepositroy<OrderDetail>
    {
        public Task<Order> GetOrderByOrderDetail(int orderDetailId);
    }
}
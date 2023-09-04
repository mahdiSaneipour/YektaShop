using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IOrderDetailRepository : IGenericRepositroy<OrderDetail>
    {
        public Task<Order> GetOrderByOrderDetail(int orderDetailId);

        public Task<List<OrderDetail>> GetAllOrdersWithIncluse(Expression<Func<OrderDetail, bool>> where = null);
    }
}
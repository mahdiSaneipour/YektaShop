using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly BNContext _context;

        public OrderDetailRepository(BNContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByOrderDetail(int orderDetailId)
        {
            return _context.OrderDetails.Where(od => od.Id == orderDetailId)
                .Include(od => od.Order).ThenInclude(o => o.Discount)
                .ThenInclude(d => d.DiscountProduct).ThenInclude(dp => dp.Product)
                .ThenInclude(p => p.Colors)
                .FirstOrDefaultAsync().Result.Order;
        }
    }
}

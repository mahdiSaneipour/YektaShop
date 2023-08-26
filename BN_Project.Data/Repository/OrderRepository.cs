using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly BNContext _context;

        public OrderRepository(BNContext context) : base (context)
        {
            _context = context;
        }

        public async Task<Order> GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId && o.Status == 0).Include(o => o.OrderDetails)
                .ThenInclude(od => od.Color).ThenInclude(c => c.Product)
                .ThenInclude(p => p.DiscountProduct).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrderBoxByStatusWithIncludeOrderDetail(OrderStatus orderStatus, int userId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).Where(o => o.Status == orderStatus && o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderWithIncludeOrderDetail(int orderId)
        {
            return await _context.Orders.Include(c => c.OrderDetails).Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetProductImagesByOrderId(int orderId)
        {
            var result = _context.OrderDetails.Include(od => od.Color)
                .ThenInclude(c => c.Product).Where(o => o.OrderId == orderId);

            return await result.Select(od => od.Color.Product.Image).ToListAsync();
        }
    }
}

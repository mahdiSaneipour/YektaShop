using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly BNContext _context;

        public OrderDetailRepository(BNContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetAllOrdersWithIncluse(Expression<Func<OrderDetail, bool>> where = null)
        {
            IQueryable<OrderDetail> _query = _context.OrderDetails;
            if (where != null)
                _query = _query.Include(od => od.Color).ThenInclude(c => c.Product).Where(where);
            return await _query.ToListAsync();
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

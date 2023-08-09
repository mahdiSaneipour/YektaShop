using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        private readonly BNContext _context;
        public DiscountRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Discount> GetDiscountWithProducts(int Id)
        {
            return await _context.Discounts.Where(n => n.Id == Id)
                .Include(n => n.DiscountProduct)
                .ThenInclude(n => n.Product)
                .FirstOrDefaultAsync();
        }
    }
}

using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class DiscountProductRepository : GenericRepository<DiscountProduct>, IDiscountProductRepository
    {
        private readonly BNContext _context;
        public DiscountProductRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

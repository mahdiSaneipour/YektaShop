using BN_Project.Data.Context;
using BN_Project.Domain.Entities.OrderBasket;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class PurchesHistoryRepository : GenericRepository<PurchesHistory>, IPurchesHistoryRepository
    {
        private readonly BNContext _context;
        public PurchesHistoryRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

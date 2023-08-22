using BN_Project.Data.Context;
using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class ImpressionRepository : GenericRepository<Impression>, IImpressionsRepository
    {
        private readonly BNContext _context;
        public ImpressionRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

using BN_Project.Data.Context;
using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class CommentStrengthRepository : GenericRepository<Strength>, ICommentStrengthRepository
    {
        private readonly BNContext _context;
        public CommentStrengthRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

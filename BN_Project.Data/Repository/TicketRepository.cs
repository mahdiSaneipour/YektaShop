using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly BNContext _context;
        public TicketRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllWithRelation(Expression<Func<Ticket, bool>> where = null)
        {
            IQueryable<Ticket> query = _context.Tickets;
            if (where != null)
                query = query.Where(where);
            return await query.Include(n => n.Section)
                 .Include(n => n.TicketMessages)
                 .AsSplitQuery()
                 .ToListAsync();
        }

        public async Task<Ticket> GetSingleWithRelation(Expression<Func<Ticket, bool>> where = null)
        {
            IQueryable<Ticket> query = _context.Tickets;
            if (where != null)
                query = query.Where(where);
            return await query.Include(n => n.Section)
                .Include(n => n.TicketMessages)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
}

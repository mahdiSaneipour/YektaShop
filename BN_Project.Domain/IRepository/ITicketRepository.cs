using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface ITicketRepository : IGenericRepositroy<Ticket>
    {
        public Task<List<Ticket>> GetAllWithRelation(Expression<Func<Ticket, bool>> where = null);
        public Task<Ticket> GetSingleWithRelation(Expression<Func<Ticket, bool>> where = null);
    }
}

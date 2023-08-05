using BN_Project.Data.Context;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepositroy<TEntity> where TEntity : class
    {
        private readonly BNContext _context;
        private readonly DbSet<TEntity> _db;
        public GenericRepository(BNContext context)
        {
            _context = context;
            _db = context.Set<TEntity>();
        }
        public void Delete(TEntity entity)
        {
            _db.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> _query = _db;
            if (where != null)
                _query = _query.Where(where);
            return await _query.ToListAsync();
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> where)
        {
            return await _db.Where(where).SingleOrDefaultAsync();
        }

        public async Task Insert(TEntity entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task<bool> IsThereAny()
        {
            return await _db.AnyAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _db.Update(entity);
        }
    }
}

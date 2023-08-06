using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IGenericRepositroy<TEntity> where TEntity : class
    {
        public void Update(TEntity entity);

        public void Delete(TEntity entity);

        public Task Insert(TEntity entity);

        public Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> where = null);

        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> where);

        public Task<bool> IsThereAny();

        public Task SaveChanges();
    }
}

using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface ICategoryRepository
    {
        public void Delete(Category category);

        public void Insert(Category category);

        public void Update(Category category);

        public Task<List<Category>> GetAll(Expression<Func<Category, bool>> where = null);

        public Task<Category> GetById(int id);

        public Task<string> GetNameById(int id);
        public Task SaveChanges();
    }
}

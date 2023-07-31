using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IProductRepository
    {
        public Task<IQueryable<Product>> GetProducts(Expression<Func<Product, bool>> where = null);

        public Task InsertProduct(Product product);

        public Task<Product> GetProductByProductId(int productId);

        public Task UpdateProduct(Product product);

        public Task SaveChanges();
    }
}

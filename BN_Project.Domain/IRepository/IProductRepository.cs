using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IProductRepository
    {
        public Task<IQueryable<Product>> GetProducts(Expression<Func<Product, bool>> where = null);

        public Task InsertProduct(Product product);

        public Task<Product> GetProductByProductId(int productId);

        public void UpdateProduct(Product product);

        public List<string> SearchProductAndReturnName(string name);

        public Task<int> GetProductIdByName(string name);

        public Task SaveChanges();
    }
}

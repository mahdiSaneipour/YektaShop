using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IProductRepository : IGenericRepositroy<Product>
    {
        public Task<List<string>> SearchProductAndReturnName(string name);
        public Task<int> GetProductIdByName(string name);
    }
}

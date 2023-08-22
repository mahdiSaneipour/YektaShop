using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IProductRepository : IGenericRepositroy<Product>
    {
        public Task<List<string>> SearchProductAndReturnName(string name);

        public Task<int> GetProductIdByName(string name);

        public Task<bool> IsProductNameExist(string name, int? productId = 0);

        public Task<Product> GetProductByIdWithIncludeCategory(int productId);

        public Task<List<Product>> GetProductsIncludeColorsByCategoryId(int categoryId);

        public Task<Product> GetProductByIdWithIncludeCategoryAndColorAndImage(int productId);

        public Task<Product> GetProductByIdWithIncludeDiscount(int productId);
    }
}

using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface ICategoryRepository : IGenericRepositroy<Category>
    {
        public Task<string> GetNameById(int id);

        public Task<int> GetParentIdBySubCategoryId(int id);

        public Task<List<Category>> GetAllCategoryWithIncludeProducts();

        public Task<bool> IsCategoryNameExist(string name, int? categoryId = 0);

        public Task<List<string>> SearchCategoriesByName(string name);

        public Task<int> GetCategoryIdByName(string name);

        public Task<string> GetCategoryNameByCategoryId(int categoryId);
    }
}

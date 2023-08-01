using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface ICategoryRepository : IGenericRepositroy<Category>
    {
        public Task<string> GetNameById(int id);

        public Task<int> GetParentIdBySubCategoryId(int id);
    }
}

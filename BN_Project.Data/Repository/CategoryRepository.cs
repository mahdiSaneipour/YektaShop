using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly BNContext _context;
        public CategoryRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<string> GetNameById(int id)
        {
            return await _context.Categories.Select(n => n.Title).FirstOrDefaultAsync();
        }

        public async Task<int> GetParentIdBySubCategoryId(int id)
        {
            return (int)await _context.Categories.Where(c => c.Id == id).Select(c => c.ParentId).FirstOrDefaultAsync();
        }
    }
}

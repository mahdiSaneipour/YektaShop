using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<List<Category>> GetAllCategoryWithIncludeProducts()
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }

        public async Task<int> GetCategoryIdByName(string name)
        {
            return await _context.Categories.Where(c => c.Title == name).Select(c => c.Id).FirstOrDefaultAsync();
        }

        public async Task<string> GetCategoryNameByCategoryId(int categoryId)
        {
            return await _context.Categories.Where(c => c.Id == categoryId).Select(c => c.Title).FirstOrDefaultAsync();
        }

        public async Task<string> GetNameById(int id)
        {
            return await _context.Categories.Select(n => n.Title).FirstOrDefaultAsync();
        }

        public async Task<int> GetParentIdBySubCategoryId(int id)
        {
            return (int)await _context.Categories.Where(c => c.Id == id).Select(c => c.ParentId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCategoryNameExist(string name, int? categoryId = 0)
        {
            return await _context.Categories.AnyAsync(c => c.Title == name && c.Id != categoryId);
        }

        public async Task<List<string>> SearchCategoriesByName(string name)
        {
            return await _context.Categories.Where(c => c.Title.Contains(name)).Select(c => c.Title).ToListAsync();
        }
    }
}

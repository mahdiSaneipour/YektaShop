using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BNContext _context;
        public CategoryRepository(BNContext context)
        {
            _context = context;
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetAll(Expression<Func<Category, bool>> where = null)
        {
            if (where == null)
            {
                return await _context.Categories
                    .Include(n => n.ParentCategory)
                    .ToListAsync();
            }
            else
            {
                return await _context.Categories.Where(where)
                    .Include(n => n.ParentCategory)
                    .ToListAsync();
            }
        }

        public async Task<Category> GetById(int Id)
        {
            return await _context.Categories
                .Include(n => n.ParentCategory)
                .SingleOrDefaultAsync(n => n.Id == Id);
        }

        public async Task<string> GetNameById(int id)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.Id == id).Result.Title.ToString();
        }

        public async Task Insert(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}

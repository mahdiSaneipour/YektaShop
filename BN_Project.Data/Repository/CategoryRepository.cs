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

        public async void Delete(int Id)
        {
            var item = await GetById(Id);
            Delete(Id);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetAll(Expression<Func<Category, bool>> where = null)
        {
            if (where == null)
            {
                return await _context.Categories.ToListAsync();
            }
            else
            {
                return await _context.Categories.Where(where).ToListAsync();
            }
        }

        public async Task<Category> GetById(int Id)
        {
            return await _context.Categories.SingleOrDefaultAsync(n => n.Id == Id);
        }

        public void Insert(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}

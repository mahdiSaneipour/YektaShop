using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class ColorRepository : IColorRepository
    {
        private readonly BNContext _context;

        public ColorRepository(BNContext context)
        {
            _context = context;
        }

        public void AddColor(Color color)
        {
            _context.Colors.Add(color);
        }

        public Color GetColorByColorId(int colorId)
        {
            return _context.Colors.FirstOrDefault(c => c.Id == colorId);
        }

        public IQueryable<Color> GetColorsByProductId(int productId)
        {
            return _context.Colors.Where(c => c.ProductId == productId);
        }

        public void UpdateColor(Color color)
        {
            _context.Colors.Update(color);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Color> GetAllColors(Expression<Func<Color, bool>> where = null)
        {
            if (where == null)
            {
                return _context.Colors.Include(c => c.Product);
            }
            else
            {
                return _context.Colors.Where(where).Include(c => c.Product);
            }
        }

        public Task<Color> GetDefaultColorByProductId(int productId)
        {
            return _context.Colors.FirstOrDefaultAsync(c => c.ProductId == productId && c.IsDefault);
        }
    }
}

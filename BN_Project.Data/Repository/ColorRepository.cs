using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        private readonly BNContext _context;

        public ColorRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetAllColorsWithProductInclude()
        {
            return await _context.Colors.Include(c => c.Product).ToListAsync();
        }

        public async Task<int> GetColorCountByColorId(int colorId)
        {
            return await _context.Colors.Where(c => c.Id == colorId).Select(c => c.Count).FirstOrDefaultAsync();
        }

        public async Task<long> GetColorPriceByColorId(int colorId)
        {
            return await _context.Colors.Where(c => c.Id == colorId).Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<Color> GetColorWithProductInclude(int colorId)
        {
            return await _context.Colors.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == colorId);
        }

        public async Task<List<string>> GetHexColorsByProductId(int productId)
        {
            return await _context.Colors.Where(c => c.ProductId == productId).Select(c => c.Hex).ToListAsync();
        }
    }
}

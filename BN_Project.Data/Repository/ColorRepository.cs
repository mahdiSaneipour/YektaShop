﻿using BN_Project.Data.Context;
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

        public async Task<bool> AnyDiscount(int colorId)
        {
            return await _context.Colors.Where(c => c.Id == colorId).Include(c => c.Product)
                .ThenInclude(c => c.DiscountProduct)
                .AnyAsync(c => c.Product.DiscountProduct == null);
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

        public async Task<int> GetDiscountPercentByColorId(int colorId)
        {
            int percent;

            if (await AnyDiscount(colorId))
            {
                /*var product = await _context.Colors.Where(c => c.co).Include(c => c.Product);*/
               /* percent = await _context.Colors.Include(c => c.Product)
                    .ThenInclude(p => p.DiscountProduct).ThenInclude(p => p.Product)
                .Where(c => c.Id == colorId).Select(c =>
                c.Product.DiscountProduct.OrderBy(c => c.Discount.Percent)
                .FirstOrDefault().Discount.Percent)
                .FirstOrDefaultAsync();*/

                percent = 100;
            } else
            {
                percent = 0;
            }

            return percent;
        }

        public async Task<List<string>> GetHexColorsByProductId(int productId)
        {
            return await _context.Colors.Where(c => c.ProductId == productId).Select(c => c.Hex).ToListAsync();
        }

        public async Task<int> GetProductIdByColorId(int colorId)
        {
            return _context.Colors.FirstOrDefaultAsync(c => c.Id == colorId).Result.ProductId;
        }
    }
}

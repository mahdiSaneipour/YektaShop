using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly BNContext _context;

        public ProductRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> SearchProductAndReturnName(string name)
        {
            return await _context.Products.Where(p => p.Name.Contains(name)).Select(p => p.Name).ToListAsync();
        }

        public async Task<int> GetProductIdByName(string name)
        {
            return await _context.Products.Where(p => p.Name == name).Select(p => p.Id).FirstOrDefaultAsync();
        }
    }
}
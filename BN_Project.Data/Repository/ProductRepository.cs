using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly BNContext _context;

        public ProductRepository(BNContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByProductId(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public async Task<IQueryable<Product>> GetProducts(Expression<Func<Product, bool>> where = null)
        {
            if(where == null)
            {
                return _context.Products;
            } else
            {
                return _context.Products.Where(where);
            }
        }

        public async Task InsertProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task SaveChanges()
        {
            _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
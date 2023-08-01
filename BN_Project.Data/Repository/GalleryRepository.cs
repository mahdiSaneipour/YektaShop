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
    public class GalleryRepository : IGalleryRepository
    {
        private readonly BNContext _context;
        public GalleryRepository(BNContext context)
        {
            _context = context;
        }

        public void Delete(ProductGallery gallery)
        {
            _context.ProductGallery.Remove(gallery);
        }

        public async Task<List<ProductGallery>> GetAll(Expression<Func<ProductGallery, bool>> where = null)
        {
            if (where == null)
            {
                return await _context.ProductGallery
                    .Include(n => n.Product)
                    .ToListAsync();
            }
            else
            {
                return await _context.ProductGallery.Where(where)
                    .Include(n => n.Product)
                    .ToListAsync();
            }
        }

        public async Task<ProductGallery> GetById(int Id)
        {
            return await _context.ProductGallery
                .Include(n => n.Product)
                .SingleOrDefaultAsync(n => n.Id == Id);
        }


        public async Task Insert(ProductGallery gallery)
        {
            await _context.ProductGallery.AddAsync(gallery);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(ProductGallery gallery)
        {
            _context.ProductGallery.Update(gallery);
        }
    }
}

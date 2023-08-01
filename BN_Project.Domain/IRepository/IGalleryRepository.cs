using BN_Project.Domain.Entities;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface IGalleryRepository
    {
        public void Delete(ProductGallery gallery);

        public Task Insert(ProductGallery gallery);

        public void Update(ProductGallery gallery);

        public Task<List<ProductGallery>> GetAll(Expression<Func<ProductGallery, bool>> where = null);

        public Task<ProductGallery> GetById(int id);
        public Task SaveChanges();
    }
}

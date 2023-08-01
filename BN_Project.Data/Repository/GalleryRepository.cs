using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class GalleryRepository : GenericRepository<ProductGallery>, IGalleryRepository
    {
        private readonly BNContext _context;
        public GalleryRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

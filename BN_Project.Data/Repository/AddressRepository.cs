using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly BNContext _context;
        public AddressRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

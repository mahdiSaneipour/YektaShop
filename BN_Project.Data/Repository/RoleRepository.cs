using BN_Project.Data.Context;
using BN_Project.Domain.Entities.Authentication;
using BN_Project.Domain.IRepository;

namespace BN_Project.Data.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRolesRepository
    {
        private readonly BNContext _context;
        public RoleRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}

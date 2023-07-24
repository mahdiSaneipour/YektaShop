using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class UserInformationRepository : IUserInformationRepository
    {
        private readonly BNContext _context;
        public UserInformationRepository(BNContext context)
        {
            _context = context;
        }
        public async Task<UserEntity> GetUserInformationByToken(int id  )
        {
            return await _context.Users.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BNContext _context;

        public AccountRepository(BNContext context)
        {
            _context = context;
        }

        public void DeleteUser(int Id)
        {
            var user = GetUserById(Id).Result;
            _context.Users.Remove(user);
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return result;
        }

        public async Task<UserEntity> GetUserById(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(n => n.Id == id);
        }

        public async Task<UserEntity> GetUserByToken(string token)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.ActivationCode == token);
            return result;
        }

        public async Task<UserEntity> RegisterUsere(UserEntity register)
        {
            await _context.Users.AddAsync(register);
            return register;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        void IAccountRepository.UpdateUser(UserEntity user)
        {
            _context.Users.Update(user);
        }
    }
}

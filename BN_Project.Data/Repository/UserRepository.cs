using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BNContext _context;

        public UserRepository(BNContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<UserEntity>> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<UserEntity> GetUserById(int Id)
        {
            return await _context.Users.SingleOrDefaultAsync(n => n.Id == Id);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber)
        {
            return _context.Users.Any(u => u.PhoneNumber == phoneNumber);
        }

        public void RemoveUser(UserEntity user)
        {
            _context.Remove(user);
        }

        async Task IUserRepository.AddUserFromAdmin(UserEntity user)
        {
            _context.Users.Add(user);
        }

        async Task IUserRepository.SaveChanges()
        {
            _context.SaveChangesAsync();
        }
    }
}

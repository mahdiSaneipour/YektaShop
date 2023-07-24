using BN_Project.Core.DTOs.User;
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
    public class AccountRepository : IAccountRepository
    {
        private readonly BNContext _context;

        public AccountRepository(BNContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return result;
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

        public Task SaveChanges()
        {
            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}

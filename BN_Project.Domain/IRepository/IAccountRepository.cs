using BN_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IAccountRepository
    {
        public Task<UserEntity> RegisterUsere(UserEntity register);

        public Task<UserEntity> GetUserByEmail(string email);

        public Task<UserEntity> GetUserByToken(string token);

        public Task SaveChanges();
    }
}

using BN_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IUserRepository
    {
        public Task<IQueryable<UserEntity>> GetAllUsers();

        public Task<bool> IsEmailExist(string email);

        public Task<bool> IsPhoneNumberExist(string phoneNumber);

        public Task AddUserFromAdmin(UserEntity user);

        public Task SaveChanges();
    }
}

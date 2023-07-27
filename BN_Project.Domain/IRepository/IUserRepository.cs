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
    }
}

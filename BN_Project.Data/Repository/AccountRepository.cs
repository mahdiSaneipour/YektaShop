using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BN_Project.Data.Repository
{
    public class AccountRepository : GenericRepository<UserEntity>, IAccountRepository
    {
        public AccountRepository(BNContext context)
            : base(context)
        {
        }
    }
}

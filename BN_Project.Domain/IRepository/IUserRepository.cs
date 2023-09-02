using BN_Project.Domain.Entities;
using BN_Project.Domain.Entities.Authentication;

namespace BN_Project.Domain.IRepository
{
    public interface IUserRepository : IGenericRepositroy<UserEntity>
    {
        public Task<bool> IsEmailExist(string email);
        public Task<bool> IsPhoneNumberExist(string phoneNumber);

        public Task<bool> IsUserHavePermission(int userId, string permission);
    }
}

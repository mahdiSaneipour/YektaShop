using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IUserRepository : IGenericRepositroy<UserEntity>
    {
        public Task<bool> IsEmailExist(string email);
        public Task<bool> IsPhoneNumberExist(string phoneNumber);
    }
}

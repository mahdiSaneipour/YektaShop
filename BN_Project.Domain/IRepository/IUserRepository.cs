using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IUserRepository
    {
        public Task<IQueryable<UserEntity>> GetAllUsers();

        public Task<bool> IsEmailExist(string email);

        public Task<bool> IsPhoneNumberExist(string phoneNumber);

        public Task AddUserFromAdmin(UserEntity user);

        public Task<UserEntity> GetUserById(int Id);
        public void RemoveUser(UserEntity user);

        public Task SaveChanges();
    }
}

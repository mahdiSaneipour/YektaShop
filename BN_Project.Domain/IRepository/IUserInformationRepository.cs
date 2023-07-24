using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IUserInformationRepository
    {
        public Task<UserEntity> GetUserInformationByToken(int id);

        public Task SaveChanges();
    }
}

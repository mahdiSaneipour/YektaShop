using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Entities.Authentication;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        private readonly BNContext _context;
        public UserRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task RemoveRole(int roleId, int userId)
        {
            var role = await _context.UsersRoles.SingleAsync(n => n.RoleId == roleId && n.UserId == userId);
            _context.Remove(role);
        }

        public async Task<List<int>> GetUserRoles(int userId)
        {
            return await _context.UsersRoles.Where(n => n.UserId == userId).Select(n => n.RoleId).ToListAsync();
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.Where(n => n.Email == email).AnyAsync() == true;
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber)
        {
            return await _context.Users.Where(n => n.PhoneNumber == phoneNumber).AnyAsync();
        }

        public async Task<bool> IsUserHavePermission(int userId, string permission)
        {
            if (await _context.Users.Where(n => n.Id == userId).SelectMany(n => n.UsersRoles).AnyAsync(n => n.RoleId == 1))
                return true;

            return await _context.Users.Where(n => n.Id == userId)
                .SelectMany(n => n.UsersRoles)
                .Select(n => n.Role)
                .SelectMany(n => n.RolesPermissions)
                .Select(n => n.Permission)
                .AnyAsync(n => n.UniqeName == permission);
        }
    }
}

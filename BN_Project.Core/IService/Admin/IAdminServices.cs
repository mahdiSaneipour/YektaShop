using BN_Project.Core.Response;
using BN_Project.Domain.ViewModel.Admin;

namespace BN_Project.Core.IService.Admin
{
    public interface IAdminServices
    {
        public Task<IReadOnlyList<UserListViewModel>> GetUsersForAdmin(int pageId);

        public Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser);
        public Task<EditUserViewModel> GetUserById(int Id);
        public Task<bool> RemoveUserById(int Id);
        public Task<BaseResponse> EditUser(EditUserViewModel user);
    }
}

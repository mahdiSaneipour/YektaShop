using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.ViewModel.Admin;

namespace BN_Project.Core.IService.Admin
{
    public interface IAdminServices
    {
        #region Users

        public Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId);

        public Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser);

        #endregion

        #region Product

        public Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1);

        #endregion
        public Task<bool> RemoveUserById(int Id);
    }
}

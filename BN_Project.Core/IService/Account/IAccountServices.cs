using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.UserProfile;

namespace BN_Project.Core.IService.Account
{
    public interface IAccountServices
    {
        public Task<DataResponse<UserEntity>> CreateUser(RegisterUserViewModel register);

        public Task<DataResponse<UserEntity>> LoginUser(LoginUserViewModel login);

        public Task<BaseResponse> IsTokenTrue(string token);

        public void UpdateUser(UpdateUserInfoViewModel user);

        public Task<DataResponse<UserInformationViewModel>> GetUserInformationById(int Id);

        public Task<DataResponse<UserEntity>> ForgotPassword(string email);

        public Task<DataResponse<UserInformationViewModel>> GetUserByEmail(string email);

        public Task<bool> CheckPassword(int id, string password);
    }
}

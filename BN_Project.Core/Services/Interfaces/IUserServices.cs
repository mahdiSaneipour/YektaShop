using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.UserProfile;

namespace BN_Project.Core.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<DataResponse<UserEntity>> CreateUser(RegisterUserViewModel register);

        public Task<DataResponse<UserEntity>> LoginUser(LoginUserViewModel login);

        public Task<BaseResponse> IsTokenTrue(string token);

        public Task<DataResponse<UserInformationViewModel>> GetUserInformationById(int Id);

        public Task<DataResponse<UserEntity>> ForgotPassword(string email);

        public Task<bool> ChangeUserPassword(UserLoginInformationViewModel user);

        public Task<bool> ResetPassword(ResetPasswordViewModel resetPassword);

        public Task<bool> CheckPassword(int id, string password);

        public void ChangeActivationCode(UserEntity user);

        public Task<bool> UpdateUserFullName(UpdateUserInfoViewModel userInfo);

        public Task<bool> UpdatePhoneNumber(UpdateUserInfoViewModel userInfo);

        public void DeleteAccount(int Id);

        public Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId);

        public Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser);

        public Task<EditUserViewModel> GetUserById(int Id);

        public Task<bool> RemoveUserById(int Id);

        public Task<BaseResponse> EditUsers(EditUserViewModel user);

        public Task<List<TicketViewModel>> GetAllTickets();

        public Task<bool> CloseTicket(int Id);
    }
}

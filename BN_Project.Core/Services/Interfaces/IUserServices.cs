using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.Product;
using BN_Project.Domain.ViewModel.UserProfile;
using BN_Project.Domain.ViewModel.UserProfile.Address;
using BN_Project.Domain.ViewModel.UserProfile.Payment;

namespace BN_Project.Core.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<BaseResponse> IsTokenTrue(string token);

        #region Login, Register, Edit
        public Task<DataResponse<UserEntity>> CreateUser(RegisterUserViewModel register);
        public void ChangeActivationCode(UserEntity user);
        public Task<DataResponse<UserEntity>> LoginUser(LoginUserViewModel login);
        public Task<DataResponse<UserInformationViewModel>> GetUserInformationById(int Id);
        public Task<DataResponse<UserEntity>> ForgotPassword(string email);
        public Task<bool> ChangeUserPassword(UserLoginInformationViewModel user);
        public Task<bool> ResetPassword(ResetPasswordViewModel resetPassword);
        public Task<bool> CheckPassword(int id, string password);
        public Task<bool> UpdateUserFullName(UpdateUserInfoViewModel userInfo);
        public Task<bool> UpdatePhoneNumber(UpdateUserInfoViewModel userInfo);
        public void DeleteAccount(int Id);
        public Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId);
        public Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser);
        public Task<EditUserViewModel> GetUserById(int Id);
        public Task<bool> RemoveUserById(int Id);
        public Task<BaseResponse> EditUsers(EditUserViewModel user);
        #endregion

        #region Tickets 
        public Task<List<TicketViewModel>> GetAllTickets();
        public Task<bool> CloseTicket(int Id);
        #endregion

        #region Addresses 
        public Task<List<GetAllAddressesViewModel>> GetAllAddresses(int userId);

        public Task AddNewAddress(AddAddressViewModel address);

        public Task RemoveAddress(int addressId);

        public Task<PickAddressViewModel> GetAllAddressesForBasket(int userId);

        public Task SetAddressDefault(int addressId);
        #endregion

        #region Payment 
        public Task<PaymentRequestViewModel> GetPaymentAddress(int addressId);
        public Task<PaymentVerifyViewModel> GetVerifyInfo(int userId);
        public Task PaymentVerify(int userId, string authority, string refId);
        #endregion

        #region Authentication 
        public Task<bool> CheckUserPermissions(int userId, string permission);
        #endregion

        public Task<List<RolesForPickViewModel>> GetRolesForUser();
    }
}

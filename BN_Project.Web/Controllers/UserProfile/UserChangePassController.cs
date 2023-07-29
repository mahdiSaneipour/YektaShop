using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.DataResponse;
using Toplearn2.Application.Tools;

namespace BN_Project.Web.Controllers.UserProfile
{
    public class UserChangePassController : Controller
    {
        private readonly IAccountServices _accountService;
        public UserChangePassController(IAccountServices AccountService)
        {
            _accountService = AccountService;
        }

        private DataResponse<UserInformationViewModel> GetCurrentUser()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _accountService.GetUserInformationById(UserId).Result;
            return user;
        }

        public UserLoginInformationViewModel UserLoginInfoVM { get; set; }

        public IActionResult Index()
        {
            return View("~/Views/UserChangePass/ChangePassword.cshtml");
        }

        public async Task<IActionResult> ChangeUserPassword()
        {
            if (UserLoginInfoVM.Password != null && UserLoginInfoVM.NewPass != null && UserLoginInfoVM.ConfirmNewPass != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();
                string EncodedPassword = UserLoginInfoVM.Password.EncodePasswordMd5();
                if (EncodedPassword == user.Data.Password)
                {
                    string EncodedNewPassword = UserLoginInfoVM.NewPass.EncodePasswordMd5();
                    updateUserVM.Password = EncodedNewPassword;
                    updateUserVM.FullName = user.Data.FullName;
                    updateUserVM.PhoneNumber = user.Data.PhoneNumber;
                    updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

                    _accountService.UpdateUser(updateUserVM);

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return Redirect("/Account/Login");
                }
                else
                {
                    ViewData["PasssWordError"] = "رمز ورود اشتباه است!";
                    return View();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}

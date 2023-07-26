using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toplearn2.Application.Tools;

namespace BN_Project.Web.Controllers.UserProfile
{
    public class UserProfileController : Controller
    {
        private readonly IAccountServices _accountService;
        public UserProfileController(IAccountServices AccountService)
        {
            _accountService = AccountService;
        }

        [BindProperty]
        public UserProfileViewModel UserProfileVM { get; set; }

        private DataResponse<UserInformationViewModel> GetCurrentUser()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _accountService.GetUserInformationById(UserId).Result;
            return user;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = GetCurrentUser();
            UserProfileVM = new UserProfileViewModel();
            UserProfileVM.UserLoginInfoVM = new UserLoginInformationViewModel();
            UserProfileVM.UserInformationVM = user.Data;

            return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
        }

        public IActionResult UpdatePhoneNumber()
        {
            if (UserProfileVM.UserInformationVM.PhoneNumber != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();

                updateUserVM.PhoneNumber = UserProfileVM.UserInformationVM.PhoneNumber;
                updateUserVM.FullName = user.Data.FullName;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
                updateUserVM.Password = user.Data.Password;

                _accountService.UpdateUser(updateUserVM);

                user.Data.PhoneNumber = updateUserVM.PhoneNumber;
                UserProfileVM.UserInformationVM = user.Data;

                return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult UpdateFullName()
        {
            if (UserProfileVM.UserInformationVM.FullName != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();

                updateUserVM.FullName = UserProfileVM.UserInformationVM.FullName;
                updateUserVM.PhoneNumber = user.Data.PhoneNumber;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
                updateUserVM.Password = user.Data.Password;

                _accountService.UpdateUser(updateUserVM);

                user.Data.FullName = updateUserVM.FullName;
                UserProfileVM.UserInformationVM = user.Data;

                return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ChangeUserPassword()
        {
            if (UserProfileVM.UserLoginInfoVM.Password != null && UserProfileVM.UserLoginInfoVM.NewPass != null && UserProfileVM.UserLoginInfoVM.ConfirmNewPass != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();
                string EncodedPassword = UserProfileVM.UserLoginInfoVM.Password.EncodePasswordMd5();
                if (EncodedPassword == user.Data.Password)
                {
                    string EncodedNewPassword = UserProfileVM.UserLoginInfoVM.NewPass.EncodePasswordMd5();
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
                    UserProfileVM = new UserProfileViewModel();
                    ViewData["PasssWordError"] = "رمز ورود اشتباه است!";
                    UserProfileVM.UserInformationVM = user.Data;
                    return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
                }
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteUserAccount()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _accountService.DeleteAccount(UserId);


            return Redirect("/Account/Login");
        }
    }
}

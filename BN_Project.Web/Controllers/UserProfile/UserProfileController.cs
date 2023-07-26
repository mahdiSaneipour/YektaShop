using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            UserProfileVM.UserInformationVM = user.Data;

            return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
        }

        public IActionResult UpdatePhoneNumber()
        {
            if (UserProfileVM.UserInformationVM.PhoneNumber != null)
            {
                var user = GetCurrentUser();
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();
                string phoneNumber = UserProfileVM.UserInformationVM.PhoneNumber;
                updateUserVM.PhoneNumber = phoneNumber;
                updateUserVM.FullName = user.Data.FullName;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
                _accountService.UpdateUser(updateUserVM);
                user.Data.PhoneNumber = phoneNumber;
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
                var user = GetCurrentUser();
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();
                string FullName = UserProfileVM.UserInformationVM.FullName;
                updateUserVM.FullName = FullName;
                updateUserVM.PhoneNumber = user.Data.PhoneNumber;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
                _accountService.UpdateUser(updateUserVM);
                user.Data.FullName = FullName;
                UserProfileVM.UserInformationVM = user.Data;
                return View("~/Views/UserProfile/Profile.cshtml", UserProfileVM);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

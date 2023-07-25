using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.ViewModel.UserProdile;
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
        public UserInformationViewModel UserInfo { get; set; }

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

            return View("~/Views/UserProfile/Profile.cshtml", user.Data);
        }

        public IActionResult UpdatePhoneNumber()
        {
            if (UserInfo.PhoneNumber != null)
            {
                var user = GetCurrentUser();
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();
                string phoneNumber = UserInfo.PhoneNumber;
                updateUserVM.PhoneNumber = phoneNumber;
                updateUserVM.


                return View("~/Views/UserProfile/Profile.cshtml", phoneNumber);
            }
            else
            {

                return NotFound();
            }
        }


    }
}

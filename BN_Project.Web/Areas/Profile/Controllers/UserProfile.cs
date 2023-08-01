using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Profile.Controllers
{
    [Area("Profile")]
    public class UserProfile : Controller
    {
        private readonly IUserServices _userServices;
        public UserProfile(IUserServices UserServices)
        {
            _userServices = UserServices;
        }

        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [BindProperty]
        public UserInformationViewModel UserInformationVM { get; set; }

        private DataResponse<UserInformationViewModel> GetCurrentUser()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _userServices.GetUserInformationById(UserId).Result;
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
            UserInformationVM = new UserInformationViewModel();
            UserInformationVM = user.Data;

            return View("Profile", UserInformationVM);
        }

        public IActionResult UpdatePhoneNumber()
        {
            if (UserInformationVM.PhoneNumber != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();

                updateUserVM.PhoneNumber = UserInformationVM.PhoneNumber;
                updateUserVM.FullName = user.Data.FullName;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

                _userServices.UpdateUser(updateUserVM);

                user.Data.PhoneNumber = updateUserVM.PhoneNumber;
                UserInformationVM = user.Data;

                return View("Profile", UserInformationVM);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult UpdateFullName()
        {
            if (UserInformationVM.FullName != null)
            {
                UpdateUserInfoViewModel updateUserVM = new UpdateUserInfoViewModel();

                var user = GetCurrentUser();

                updateUserVM.FullName = UserInformationVM.FullName;
                updateUserVM.PhoneNumber = user.Data.PhoneNumber;
                updateUserVM.Id = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

                _userServices.UpdateUser(updateUserVM);

                user.Data.FullName = updateUserVM.FullName;
                UserInformationVM = user.Data;

                return View("Profile", UserInformationVM);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Profile.Controllers
{
    [Authorize]
    [Area("UserProfile")]
    [Route("Controller")]
    public class UserProfileController : Controller
    {
        private readonly IUserServices _userServicess;
        private readonly ITicketServices _profileServices;

        public UserProfileController(IUserServices UserServices,
            ITicketServices profileServices)
        {
            _userServicess = UserServices;
            _profileServices = profileServices;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [NonAction]
        private DataResponse<UserInformationViewModel> GetCurrentUser()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _userServicess.GetUserInformationById(UserId).Result;
            return user;
        }
        [NonAction]
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            var user = GetCurrentUser();
            UserInformationViewModel UserInformationVM = new UserInformationViewModel();
            UserInformationVM = user.Data;

            return View(nameof(Profile), UserInformationVM);
        }

        #region EditProfileInfo

        [HttpPost, ValidateAntiForgeryToken]
        [Route("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber(UserInformationViewModel UserInformationVM)
        {
            UpdateUserInfoViewModel user = new UpdateUserInfoViewModel()
            {
                Id = GetCurrentUserId(),
                PhoneNumber = UserInformationVM.PhoneNumber
            };
            if (await _userServicess.UpdatePhoneNumber(user))
            {
                return Redirect(nameof(Profile));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("UpdateFullName")]
        public async Task<IActionResult> UpdateFullName(UserInformationViewModel UserInformationVM)
        {
            UpdateUserInfoViewModel user = new UpdateUserInfoViewModel()
            {
                Id = GetCurrentUserId(),
                FullName = UserInformationVM.FullName
            };

            if (await _userServicess.UpdateUserFullName(user))
            {
                return Redirect("Profile");
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region EditProfilePassword

        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            UserLoginInformationViewModel model = new UserLoginInformationViewModel()
            {
                UserId = GetCurrentUserId()
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserLoginInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userServicess.ChangeUserPassword(model);

            if (result)
            {
                return Redirect("~/Controller/Logout");
            }

            ModelState.AddModelError("Password", "رمز عبور صحیح نمیباشد");
            return View();
        }
        #endregion

        #region Order

        [Route("Orders")]
        public async Task<IActionResult> Orders()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var result = await _profileServices.GetBoxOrderList(OrderStatus.AwaitingPayment, userId);
            
            if (result.Status == Status.Success)
            {
                return View(result.Data.FirstOrDefault());
            } else if (result.Status == Status.NotFound)
            {
                return View(result);
            }

            return RedirectToAction("Profile");
        }


        [HttpGet("Profile/UserProfile/OtherOrders/{orderStatus}")]
        public async Task<IActionResult> OtherOrders(OrderStatus orderStatus)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

            var result = await _profileServices.GetBoxOrderList(orderStatus, userId);
            return PartialView("../Shared/Profile/_OtherOrdersPartialView", result.Data);
        }

        #endregion
    }
}
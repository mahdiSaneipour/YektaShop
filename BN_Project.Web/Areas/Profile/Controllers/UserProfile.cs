using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Enum.Ticket;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BN_Project.Web.Areas.Profile.Controllers
{
    [Authorize]
    [Area("Profile")]
    public class UserProfile : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IProfileService _profileServices;

        public UserProfile(IUserServices UserServices,
            IProfileService profileServices)
        {
            _userServices = UserServices;
            _profileServices = profileServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        private DataResponse<UserInformationViewModel> GetCurrentUser()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _userServices.GetUserInformationById(UserId).Result;
            return user;
        }
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        public IActionResult Profile()
        {
            var user = GetCurrentUser();
            UserInformationViewModel UserInformationVM = new UserInformationViewModel();
            UserInformationVM = user.Data;

            return View("Profile", UserInformationVM);
        }

        #region EditProfileInfo
        public async Task<IActionResult> UpdatePhoneNumber(UserInformationViewModel UserInformationVM)
        {
            UpdateUserInfoViewModel user = new UpdateUserInfoViewModel()
            {
                Id = GetCurrentUserId(),
                PhoneNumber = UserInformationVM.PhoneNumber
            };
            if (await _userServices.UpdatePhoneNumber(user))
            {
                return Redirect("Profile");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> UpdateFullName(UserInformationViewModel UserInformationVM)
        {
            UpdateUserInfoViewModel user = new UpdateUserInfoViewModel()
            {
                Id = GetCurrentUserId(),
                FullName = UserInformationVM.FullName
            };

            if (await _userServices.UpdateUserFullName(user))
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
        public async Task<IActionResult> ChangePassword()
        {
            UserLoginInformationViewModel model = new UserLoginInformationViewModel()
            {
                UserId = GetCurrentUserId()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserLoginInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userServices.ChangeUserPassword(model);

            if (result)
            {
                return Redirect("~/Account/Account/Logout");
            }

            ModelState.AddModelError("Password", "رمز عبور صحیح نمیباشد");
            return View();
        }
        #endregion

        #region Tickets
        public async Task<IActionResult> Tickets()
        {
            var items = await _profileServices.GetAllTickets();
            return View(items);
        }

        public async Task<IActionResult> AddTicket()
        {
            AddTicketViewModel addTicket = new AddTicketViewModel();
            addTicket.Sections = await _profileServices.GetAllSectionsName();
            addTicket.OwnerId = GetCurrentUserId();
            return View(addTicket);
        }
        [HttpPost]
        public async Task<IActionResult> AddTicket(AddTicketViewModel addTicket)
        {
            if (await _profileServices.AddNewTicket(addTicket))
            {
                return RedirectToAction("Tickets", "UserProfile");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> TicketDetails(int Id)
        {
            if (Id == 0)
                return NotFound();
            var item = await _profileServices.GetTicketMessages(Id);
            item.AddMessage = new AddMessageViewModel();
            item.AddMessage.TicketId = Id;
            item.AddMessage.SenderId = GetCurrentUserId();
            return View(item);
        }

        public async Task<IActionResult> SendMessage(TicketMessagesViewModel message)
        {
            if (await _profileServices.AddMessageForTicket(message.AddMessage))
            {
                return RedirectToAction("TicketDetails", new { Id = message.AddMessage.TicketId });
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}

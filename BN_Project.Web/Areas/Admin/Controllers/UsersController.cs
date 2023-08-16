using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("[Controller]")]
    public class UsersController : Controller
    {
        private readonly IUserServices _userServices;
        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        #region Users

        [Route("Users")]
        public async Task<IActionResult> Users(int pageId = 1)
        {
            var result = await _userServices.GetUsersForAdmin(pageId);

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        [Route("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(AddUserViewModel addUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userServices.AddUserFromAdmin(addUser);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                    ModelState.AddModelError("Email", result.Message);
                    return View();

                case Status.AlreadyHavePhoneNumber:
                    ModelState.AddModelError("PhoneNumber", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }
        }

        [Route("RemoveUser")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            if (await _userServices.RemoveUserById(userId))
            {
                return RedirectToAction(nameof(Users));
            }
            else
            {
                return NotFound();
            }
        }

        [Route("EditUser")]
        public async Task<IActionResult> EditUser(int userId)
        {
            var item = await _userServices.GetUserById(userId);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userServices.EditUsers(user);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                    ModelState.AddModelError("Email", result.Message);
                    return View();

                case Status.AlreadyHavePhoneNumber:
                    ModelState.AddModelError("PhoneNumber", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                return RedirectToAction(nameof(Users));
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }
        }
        #endregion
    }
}

using BN_Project.Core.Attributes;
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
        [PermissionCheker("Users_Users")]
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
        [PermissionCheker("AddUser_Users")]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            AddUserViewModel addUser = new AddUserViewModel()
            {
                Roles = await _userServices.GetRolesForUser()
            };
            return View(addUser);
        }
        [PermissionCheker("AddUser_Users")]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(AddUserViewModel addUser)
        {
            if (!ModelState.IsValid)
            {
                addUser.Roles = await _userServices.GetRolesForUser();
                return View(addUser);
            }

            var result = await _userServices.AddUserFromAdmin(addUser);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                    ModelState.AddModelError("Email", result.Message);
                    addUser.Roles = await _userServices.GetRolesForUser();
                    return View(addUser);

                case Status.AlreadyHavePhoneNumber:
                    ModelState.AddModelError("PhoneNumber", result.Message);
                    addUser.Roles = await _userServices.GetRolesForUser();
                    return View(addUser);
            }

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                addUser.Roles = await _userServices.GetRolesForUser();
                return View(addUser);
            }
        }
        [PermissionCheker("RemoveUser_Users")]
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
        [PermissionCheker("EditUser_Users")]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(int userId)
        {
            EditUserViewModel item = await _userServices.GetUserById(userId);
            return View(item);
        }
        [PermissionCheker("EditUser_Users")]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
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

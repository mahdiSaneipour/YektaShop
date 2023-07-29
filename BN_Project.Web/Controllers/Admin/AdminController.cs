using BN_Project.Core.IService.Admin;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users(int pageId = 1)
        {
            var users = await _adminServices.GetUsersForAdmin(pageId);

            return View(users);
        }

        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _adminServices.AddUserFromAdmin(addUser);

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

        public async Task<IActionResult> RemoveUser(int Id)
        {
            if (await _adminServices.RemoveUserById(Id))
            {
                return RedirectToAction("Users", "Admin");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditUser(int Id)
        {
            return View();
        }

    }
}

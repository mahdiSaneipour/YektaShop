using BN_Project.Core.DTOs.User;
using BN_Project.Core.IService.Account;
using Microsoft.AspNetCore.Mvc;
using BN_Project.Core.Response.Status;

namespace BN_Project.Web.Controllers.Account
{
    public class LoginController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(IAccountServices accountServices, IHttpContextAccessor contextAccessor)
        {
            _accountServices = accountServices;
            _contextAccessor = contextAccessor;
        }

        public async Task <IActionResult> Index()
        {
            return View("../Account/Register/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUser login)
        {
            var result = await _accountServices.LoginUser(login);

            switch(result.Status)
            {
                case Status.NotFound:
                    ModelState.AddModelError("Email", result.Message);
                    return View("../Account/Register/Register");

                case Status.NotMatch:
                    ModelState.AddModelError("Password", result.Message);
                    return View("../Account/Register/Register");
            }
            
            if (result.Status == Core.Response.Status.Status.Success)
            {
               /* string cookie = _contextAccessor.HttpContext*/

                return Redirect("/");
            } else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View("../Account/Register/Register");
            }
        }
    }
}

using BN_Project.Core.DTOs.User;
using BN_Project.Core.IService.Account;
using Microsoft.AspNetCore.Mvc;
using BN_Project.Core.Response.Status;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BN_Project.Web.Controllers.Account
{
    public class LoginController : Controller
    {
        private readonly IAccountServices _accountServices;

        public LoginController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public async Task <IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View("../Account/Login/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUser login)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var result = await _accountServices.LoginUser(login);

            switch(result.Status)
            {
                case Status.NotFound:
                    ModelState.AddModelError("Email", result.Message);
                    return View("../Account/Login/Login");

                case Status.NotMatch:
                    ModelState.AddModelError("Password", result.Message);
                    return View("../Account/Login/Login");
            }
            
            if (result.Status == Core.Response.Status.Status.Success)
            {
                #region Cookie

                var claims = new List<Claim>()
                {
                    new Claim (ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                    /*new Claim (ClaimTypes.Name, result.Data.Name)*/
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, properties);

                #endregion

                return Redirect("/");
            } else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View("../Account/Login/Login");
            }
        }


    }
}

using BN_Project.Core.ExtraViewModels;
using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.ViewModel.Account;
using EP.Core.Tools.RenderViewToString;
using EP.Core.Tools.Senders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BN_Project.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly IViewRenderService _viewRenderService;

        public AccountController(IAccountServices accountServices, IViewRenderService viewRenderService)
        {
            _accountServices = accountServices;
            _viewRenderService = viewRenderService;
        }

        #region Login

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            /*if (ModelState.IsValid)
            {
                return View();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("/");
            }*/

            var result = await _accountServices.LoginUser(login);

            switch (result.Status)
            {
                case Status.NotFound:
                    ModelState.AddModelError("Email", result.Message);
                    return View();

                case Status.NotMatch:
                    ModelState.AddModelError("Password", result.Message);
                    return View();

                case Status.NotActive:
                    ModelState.AddModelError("Email", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
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
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }
        }

        #endregion

        #region Register
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {

            var result = await _accountServices.CreateUser(register);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                case Status.Error:
                    ModelState.AddModelError("Email", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                #region Send Email

                var confirmLink = $"{Request.Scheme}://{Request.Host}/Register/ConfirmEmail?token={result.Data.ActivationCode}";

                ConfirmEmailViewModel model = new ConfirmEmailViewModel();

                model.Name = result.Data.Email;
                model.Url = confirmLink;
                string html = _viewRenderService.RenderToStringAsync("ExternalView/Email/ConfirmEmailView", model);

                try
                {
                    SendEmail.Send(result.Data.Email, "تایید ایمیل", html);
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync("Error : " + e.Message);
                    ModelState.AddModelError("Email", "ایمیل وارد شده معتبر نمیباشد");
                    return View();
                }

                return await Task.FromResult(RedirectToAction("Login"));

                #endregion
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }

        }
        #endregion

        #region Confirmation
        public IActionResult ConfirmEmail(string token)
        {

            _accountServices.IsTokenTrue(token);

            return Redirect("/");
        }
        #endregion

        #region Logout

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        #endregion

        #region ForgotPassword

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var result = await _accountServices.ForgotPassword(forgotPassword.Email);

            switch (result.Status)
            {
                case Status.NotValid:
                    ModelState.AddModelError("Email", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                #region SendEmail

                var confirmLink = $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={result.Data.ActivationCode}";

                ConfirmEmailViewModel model = new ConfirmEmailViewModel();

                model.Name = result.Data.Email;
                model.Url = confirmLink;
                string html = _viewRenderService.RenderToStringAsync("ExternalView/Email/ConfirmEmailView", model);


                try
                {
                    SendEmail.Send(result.Data.Email, "فراموشی رمز عبور", html);

                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync("Error : " + e.Message);

                    ModelState.AddModelError("Email", "ایمیل وارد شده معتبر نمیباشد");
                    return View();
                }


                #endregion

                return await Task.FromResult(Redirect("/"));
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده است, لطفا بعدا تلاش کنید");
                return View();
            }
        }

        #endregion

        #region ResetPassword

        public async Task<IActionResult> ResetPassword(string token)
        {
            var result = await _accountServices.IsTokenTrue(token);

            if (result.Status == Status.Success)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel()
                {
                    Token = token
                };

                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var result = await _accountServices.ResetPassword(resetPassword);

            if (!result)
            {
                return Redirect("/");
            }

            return RedirectToAction("Login");
        }

        #endregion
    }
}

using BN_Project.Core.ExtraViewModels;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
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
    [Route("Controller")]
    public class AccountController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly IUserServices _userServicess;

        public AccountController(
            IViewRenderService viewRenderService,
            IUserServices userServices)
        {
            _viewRenderService = viewRenderService;
            _userServicess = userServices;
        }

        #region Login

        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("Login")]
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

            var result = await _userServicess.LoginUser(login);

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

        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {

            var result = await _userServicess.CreateUser(register);

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

        [Route("ConfirmEmail")]
        public IActionResult ConfirmEmail(string token)
        {

            _userServicess.IsTokenTrue(token);

            return Redirect("/");
        }
        #endregion

        #region Logout

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        #endregion

        #region ForgotPassword

        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var result = await _userServicess.ForgotPassword(forgotPassword.Email);

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

        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token)
        {
            var result = await _userServicess.IsTokenTrue(token);

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

        [HttpPost, ValidateAntiForgeryToken]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var result = await _userServicess.ResetPassword(resetPassword);

            if (!result)
            {
                return Redirect("/");
            }

            return RedirectToAction("Login");
        }

        #endregion
    }
}

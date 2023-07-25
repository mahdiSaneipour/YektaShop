using BN_Project.Core.ExtraViewModels;
using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.ViewModel.Account;
using EP.Core.Tools.RenderViewToString;
using EP.Core.Tools.Senders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers.Account
{
    public class RegisterController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly IViewRenderService _viewRenderService;

        public RegisterController(IAccountServices accountServices, IViewRenderService viewRenderService)
        {
            _accountServices = accountServices;
            _viewRenderService = viewRenderService;
        }
        #region Register
        public async Task<IActionResult> Index()
        {
            return View("../Account/Register/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterUserViewModel register)
        {

            var result = await _accountServices.CreateUser(register);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                case Status.Error:
                    ModelState.AddModelError("Email", result.Message);
                    return View("../Account/Register/Register");
            }

            if (result.Status == Status.Success)
            {
                #region Send Email

                var confirmLink = $"{this.Request.Scheme}://{this.Request.Host}/Register/ConfirmEmail?token={result.Data.ActivationCode}";

                ConfirmEmailViewModel model = new ConfirmEmailViewModel();

                model.Name = result.Data.Name;
                model.Url = confirmLink;

                string html = _viewRenderService.RenderToStringAsync("ExternalView/Email/ConfirmEmailView", model);

                SendEmail.Send(result.Data.Email, "تایید ایمیل", html);

                return await Task.FromResult(Redirect("/Login"));

                #endregion
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View("../Account/Register/Register");
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
        #region ResetPassWord
        public async Task<IActionResult> ResetPassword()
        {

            return View();
        }
        #endregion

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login");
        }

    }
}

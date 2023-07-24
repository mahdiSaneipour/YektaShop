using BN_Project.Core.DTOs.User;
using BN_Project.Core.IService.Account;
using Microsoft.AspNetCore.Mvc;
using BN_Project.Core.Response.Status;
using System.Threading.Tasks;
using BN_Project.Core.ExtraViewModels;
using EP.Core.Tools.Senders;
using Microsoft.AspNetCore.Identity;
using Toplearn2.Application.Tools;
using EP.Core.Tools.RenderViewToString;

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

        public async Task<IActionResult> Index()
        {
            return View("../Account/Register/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterUser register)
        {
            
            var result = await _accountServices.CreateUser(register);

            switch(result.Status)
            {
                case Status.AlreadyHave:
                case Status.Error:
                    ModelState.AddModelError("Email", result.Message);
                    return View("../Account/Register/Register");
            }

            if(result.Status == Status.Success)
            {

                #region Send Email

                var confirmLink = $"{this.Request.Scheme}://{this.Request.Host}/Register/ConfirmEmail?token={result.Data.ActivationCode}";

                ConfirmEmailViewModel model = new ConfirmEmailViewModel();

                model.Name = result.Data.Name;
                model.Url = confirmLink;

                string html = _viewRenderService.RenderToStringAsync("ExternalView/Email/ConfirmEmailView", model);

                SendEmail.Send(result.Data.Email, "تایید ایمیل", html);

                return await Task.FromResult(Redirect("/"));

                #endregion
            } else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View("../Account/Register/Register");
            }

        }

        public async Task<IActionResult> ConfirmEmail(string token)
        {

            _accountServices.IsTokenTrue(token);

            return Redirect("/");
        }
    }
}

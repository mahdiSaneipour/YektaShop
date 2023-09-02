using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BN_Project.Core.Attributes
{
    public class PermissionChekerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission = "";
        private IUserServices _userServices;
        public PermissionChekerAttribute(string permission)
        {
            _permission = permission;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                _userServices = (IUserServices)
                    context.HttpContext.RequestServices
                    .GetService(typeof(IUserServices));

                int userId = Convert.ToInt32(context.HttpContext
                    .User.Claims.FirstOrDefault().Value);

                bool result = _userServices.CheckUserPermissions(userId, _permission).Result;



                if (result == false)
                {
                    context.Result = new RedirectResult("/AccessDenied");
                    await context.Result.ExecuteResultAsync(context);
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
                await context.Result.ExecuteResultAsync(context);
            }
        }
    }
}

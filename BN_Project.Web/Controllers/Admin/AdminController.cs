using BN_Project.Core.IService.Admin;
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
    }
}

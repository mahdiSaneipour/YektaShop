using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers.UserProfile
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Profile()
        {


            return View();
        }
    }
}

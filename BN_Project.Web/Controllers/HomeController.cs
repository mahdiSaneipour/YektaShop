using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

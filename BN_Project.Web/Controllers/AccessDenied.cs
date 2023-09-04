using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers
{
    public class AccessDenied : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

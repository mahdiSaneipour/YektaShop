using BN_Project.Core.DTOs.UserProfile;
using BN_Project.Core.IService.Account;
using BN_Project.Core.Response.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers.UserProfile
{
    public class UserProfileController : Controller
    {
        private readonly IAccountServices _db;
        public UserProfileController(IAccountServices db)
        {
            _db = db;
        }

        [BindProperty]
        public UserInformation UserInfo { get; set; }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            string Email = User.Identity.Name;
            var user = _db.GetUserByEmail(Email).Result;
            if (user.Status == Status.NotFound)
                return NotFound();

            return View();
        }
    }
}

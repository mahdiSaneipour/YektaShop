using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.UserProfile.Controllers
{
    [Authorize]
    [Area("UserProfile")]
    [Route("[Controller]")]
    public class AddressesController : Controller
    {
        private readonly IUserServices _userServices;
        public AddressesController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [NonAction]
        private int GetCurrentUserId()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return userId;
        }

        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var addresses = await _userServices.GetAllAddresses(userId);
            return View(addresses);
        }


        [Route("AddAddress")]
        public IActionResult AddAddress()
        {
            int UserId = GetCurrentUserId();
            AddAddressViewModel address = new AddAddressViewModel() { UserId = UserId };

            return View(address);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddAddress")]
        public async Task<IActionResult> AddAddress(AddAddressViewModel address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }

            await _userServices.AddNewAddress(address);

            return RedirectToAction(nameof(Index));
        }

        [Route("RemoveAddress/{Id}")]
        public async Task<IActionResult> RemoveAddress(int Id)
        {
            await _userServices.RemoveAddress(Id);
            return RedirectToAction(nameof(Index));
        }

        [Route("SetDefault/{Id}")]
        public async Task<IActionResult> SetDefault(int Id)
        {
            await _userServices.SetAddressDefault(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZarinPal;

namespace BN_Project.Web.Areas.Profile.Controllers
{
    [Authorize]
    [Area("UserProfile")]
    [Route("[Controller]")]
    public class PaymentRequestController : Controller
    {
        private readonly IUserServices _userServices;
        public PaymentRequestController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [NonAction]
        private int GetCurrentUserId()
        {
            int result = 0;
            result = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return result;
        }

        [Route("PickAddress")]
        public async Task<IActionResult> PickAddress()
        {
            int userId = GetCurrentUserId();
            var address = await _userServices.GetAllAddressesForBasket(userId);
            return View(address);
        }

        [Route("Pay")]
        public async Task<IActionResult> Pay(PaymentRequestViewModel pay)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();

            PaymentRequestResponse Status = await zp.PaymentRequestAsync(pay.MerchentId,
                pay.Price,
                pay.Description,
                pay.Email,
                pay.AdminPhoneNumber,
                pay.RedirectAddress);
            if (Status.Body.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Status.Body.Authority);
            }
            else
            {
                ViewData["State2"] = "عملیات با شکست مواجه شد!";
                return NotFound();
            }
        }

        public async Task<IActionResult> Verify(string Authority, string Status)
        {
            string MerchantId = "";
            int Amount = 200;
            if (Status is "OK")
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();

                PaymentVerificationResponse State = await zp.PaymentVerificationAsync(MerchantId,
                    Authority,
                    Amount);
                return View(State);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

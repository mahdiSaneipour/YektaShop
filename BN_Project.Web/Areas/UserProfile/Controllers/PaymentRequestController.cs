using BN_Project.Core.Services.Interfaces;
using BN_Project.Core.Tools;
using BN_Project.Domain.ViewModel.UserProfile.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        public async Task<IActionResult> Pay(int AddressId)
        {
            var pay = await _userServices.GetPaymentAddress(AddressId);
            pay.Email = User.FindFirstValue(ClaimTypes.Email);
            pay.RedirectAddress = "https://localhost:44309/PaymentRequest/Verify";

            System.Net.ServicePointManager.Expect100Continue = false;
            PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();

            PaymentRequestResponse Status = await zp.PaymentRequestAsync(pay.MerchentId,
                pay.Price,
                pay.Description,
                pay.Email,
                pay.PhoneNumber,
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

        [Route("Verify")]
        public async Task<IActionResult> Verify(string Authority, string Status)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var payInfo = await _userServices.GetVerifyInfo(userId);
            string MerchantId = SD.ZarinPallCode;
            if (Status is "OK")
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();

                PaymentVerificationResponse State = await zp.PaymentVerificationAsync(MerchantId,
                    Authority,
                    payInfo.Price);

                if (State.Body.Status == 100)
                {
                    PaymentResultViewModel result = new PaymentResultViewModel()
                    {
                        RefId = State.Body.RefID.ToString(),
                        Time = DateTime.Now.ConvertToShamsi(),
                        Date = DateTime.Now.ToString("HH:mm:ss")
                    };

                    await _userServices.PaymentVerify(userId, Authority, State.Body.RefID.ToString());

                    return View("PaymentSucced", result);
                }
                else
                {
                    PaymentResultViewModel result = new PaymentResultViewModel()
                    {
                        RefId = State.Body.RefID.ToString(),
                        Time = DateTime.Now.ConvertToShamsi(),
                        Date = DateTime.Now.ToString("HH:mm:ss")
                    };
                    return View("PaymentFailed", result);
                }
            }
            else
            {
                PaymentResultViewModel result = new PaymentResultViewModel();
                return View("PaymentFailed", result);
            }
        }
    }
}

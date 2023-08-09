using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Mvc;
using ZarinPal;

namespace BN_Project.Web.Areas.Profile.Controllers
{
    public class PaymentRequestController : Controller
    {
        public async Task<IActionResult> Index(PaymentRequestViewModel pay)
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

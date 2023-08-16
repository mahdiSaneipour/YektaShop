using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Ticket.Controllers
{
    [Authorize]
    [Area("UserProfile")]
    [Route("[Controller]")]
    public class TicketController : Controller
    {
        private readonly ITicketServices _ticketServices;
        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }
        [NonAction]
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        #region Ticket
        [Route("Tickets")]
        public async Task<IActionResult> Tickets()
        {
            var items = await _ticketServices.GetAllTickets();
            return View(items);
        }

        [Route("AddTicket")]
        public async Task<IActionResult> AddTicket()
        {
            AddTicketViewModel addTicket = new AddTicketViewModel();
            addTicket.Sections = await _ticketServices.GetAllSectionsName();
            addTicket.OwnerId = GetCurrentUserId();
            return View(addTicket);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddTicket")]
        public async Task<IActionResult> AddTicket(AddTicketViewModel addTicket)
        {
            if (await _ticketServices.AddNewTicket(addTicket))
            {
                return RedirectToAction(nameof(Tickets));
            }
            else
            {
                return NotFound();
            }
        }

        [Route("TicketDetails")]
        public async Task<IActionResult> TicketDetails(int Id)
        {
            if (Id == 0)
                return NotFound();
            var item = await _ticketServices.GetTicketMessages(Id);
            item.AddMessage = new AddMessageViewModel();
            item.AddMessage.TicketId = Id;
            item.AddMessage.SenderId = GetCurrentUserId();
            return View(nameof(TicketDetails), item);
        }

        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(TicketMessagesViewModel message)
        {
            if (await _ticketServices.AddMessageForTicket(message.AddMessage))
            {
                return RedirectToAction(nameof(TicketDetails), new { Id = message.AddMessage.TicketId });
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}

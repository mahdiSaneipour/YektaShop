using BN_Project.Core.Attributes;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Admin.Controllers
{
    [PermissionCheker("Tickets_Management")]
    [Area("Admin")]
    [Route("[Controller]")]
    public class TicketsController : Controller
    {
        private readonly ITicketServices _ticketServices;
        private readonly IUserServices _userServicess;
        public TicketsController(ITicketServices ticketServices,
            IUserServices userServices)
        {
            _ticketServices = ticketServices;
            _userServicess = userServices;
        }

        [NonAction]
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        #region Tickets
        [PermissionCheker("Tickets_Tickets")]
        [Route("Tickets")]
        public async Task<IActionResult> Tickets()
        {
            var items = await _userServicess.GetAllTickets();
            return View(items);
        }
        [PermissionCheker("CloseTicket_Tickets")]
        [Route("CloseTicket")]
        public async Task<IActionResult> CloseTicket(int Id)
        {
            if (await _userServicess.CloseTicket(Id))
            {
                return RedirectToAction(nameof(Tickets));
            }
            else
            {
                return NotFound();
            }
        }
        [PermissionCheker("AddTicketMessage_Tickets")]
        [Route("AddTicketMessages")]
        public async Task<IActionResult> AddTicketMessages(int Id)
        {
            if (Id == 0)
                return NotFound();
            var item = await _ticketServices.GetTicketMessages(Id);
            item.AddMessage = new AddMessageViewModel();
            item.AddMessage.TicketId = Id;
            item.AddMessage.SenderId = GetCurrentUserId();
            return View(item);
        }
        [PermissionCheker("SendMessage_Tickets")]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(TicketMessagesViewModel Message)
        {
            if (await _ticketServices.AddMessageForTicketFromAdmin(Message.AddMessage))
            {
                return RedirectToAction(nameof(AddTicketMessages), new { Id = Message.AddMessage.TicketId });
            }
            else
            {
                return NotFound();
            }

        }
        [PermissionCheker("AddTicket_Tickets")]
        [Route("AddTicket")]
        public async Task<IActionResult> AddTicket(int Id)
        {
            if (Id == 0)
                return NotFound();
            AddTicketViewModel addTicket = new AddTicketViewModel();
            addTicket.Sections = await _ticketServices.GetAllSectionsName();
            addTicket.OwnerId = Id;
            addTicket.SenderId = GetCurrentUserId();
            return View(addTicket);
        }
        [PermissionCheker("AddTicket_Tickets")]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddTicket")]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (await _ticketServices.AddNewTicketAdmin(ticket))
            {
                return RedirectToAction(nameof(Tickets));
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}

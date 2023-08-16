using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.Enum.Ticket
{
    public enum TicketStatus
    {
        [Display(Name = "درحال بررسی")]
        Pending,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته")]
        Closed
    }
}
 
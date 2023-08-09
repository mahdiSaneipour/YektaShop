using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.Enum.Ticket
{
    public enum TicketStatus
    {
        [Display(Name = "درحال بررسی")]
        Status1,
        [Display(Name = "پاسخ داده شده")]
        Status2,
        [Display(Name = "بسته")]
        Status3
    }
}
 
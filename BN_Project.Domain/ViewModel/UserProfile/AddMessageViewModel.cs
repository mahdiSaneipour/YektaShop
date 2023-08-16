using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class AddMessageViewModel
    {
        public int TicketId { get; set; }
        public int SenderId { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "پیام")]
        public string Message { get; set; }
    }
}

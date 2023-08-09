using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class AddTicketViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int SectionId { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Priority { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string Message { get; set; }
        public List<SectionForTicketViewModel> Sections { get; set; }
        public int OwnerId { get; set; }
    }
}

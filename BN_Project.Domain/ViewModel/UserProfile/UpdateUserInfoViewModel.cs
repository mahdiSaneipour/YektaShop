using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class UpdateUserInfoViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Id { get; set; }

        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}

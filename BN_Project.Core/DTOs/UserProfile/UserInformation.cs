using System.ComponentModel.DataAnnotations;

namespace BN_Project.Core.DTOs.UserProfile
{
    public class UserInformation
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}

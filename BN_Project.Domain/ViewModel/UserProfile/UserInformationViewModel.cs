using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class UserInformationViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [EmailAddress(ErrorMessage = "فرمت معتبر نمیباشد, لطفا از فرمت درست استفاده کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}

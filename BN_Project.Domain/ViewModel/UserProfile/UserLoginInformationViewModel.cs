using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class UserLoginInformationViewModel
    {
        public int UserId { get; set; }
        [DisplayName("رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string Password { get; set; }
        [DisplayName("رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string NewPass { get; set; }

        [DisplayName("تکرار رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        [Compare(nameof(NewPass), ErrorMessage = "{0} با {1} یکسان نمیباشند")]
        public string ConfirmNewPass { get; set; }
    }
}

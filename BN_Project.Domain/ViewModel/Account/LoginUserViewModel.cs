using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Account
{
    public class LoginUserViewModel
    {
        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Email { get; set; }

        [DisplayName("رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string Password { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Core.DTOs.User
{
    public class RegisterUser
    {
        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [EmailAddress(ErrorMessage = "فرمت معتبر نمیباشد, لطفا از فرمت درست استفاده کنید")]
        public string Email { get; set; }

        [DisplayName("رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string Password { get; set; }

        [DisplayName("تکرار رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        [Compare(nameof(Password), ErrorMessage = "{0} با {1} یکسان نمیباشند")]
        public string ConfirmPassword { get; set; }
    }
}

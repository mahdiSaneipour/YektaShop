using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [DisplayName("نام")]
        public string? Name { get; set; }

        [DisplayName("شماره تلفن")]
        public string? PhoneNumber { get; set; }

        [DisplayName("آواتار")]
        public string? Avatar { get; set; }

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [EmailAddress(ErrorMessage = "فرمت معتبر نمیباشد, لطفا از فرمت درست استفاده کنید")]
        public string Email { get; set; }

        [DisplayName("رمز عبور")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string Password { get; set; }

        [DisplayName("تکرار رمز عبور")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        [Compare(nameof(Password), ErrorMessage = "{0} با {1} یکسان نمیباشند")]
        public string ConfirmPassword { get; set; }
    }
}

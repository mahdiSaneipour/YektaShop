using BN_Project.Domain.ViewModel.Product;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class AddUserViewModel
    {
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
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        public string Password { get; set; }

        [DisplayName("تکرار رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "حداقل حروف رمز 8 رقم میباشد")]
        [Compare(nameof(Password), ErrorMessage = "{0} با {1} یکسان نمیباشند")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public List<int>? SelectedRoles { get; set; }
        public List<RolesForPickViewModel>? Roles { get; set; }
    }
}

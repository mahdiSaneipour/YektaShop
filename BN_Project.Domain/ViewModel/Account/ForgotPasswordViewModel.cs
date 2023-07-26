using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "فیلد {0} ظروری میباشد")]
        [EmailAddress(ErrorMessage = "فرمت مورد نظر معتبر نمیباشد")]
        public string Email { get; set; }
    }
}

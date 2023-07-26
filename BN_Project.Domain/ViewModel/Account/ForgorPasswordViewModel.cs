using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Account
{
    public class ForgorPasswordViewModel
    {
        [Required(ErrorMessage = "فیلد {0} ظروری میباشد")]
        [EmailAddress(ErrorMessage = "فرمت مورد نظر معتبر نمیباشد")]
        public string Email { get; set; }
    }
}

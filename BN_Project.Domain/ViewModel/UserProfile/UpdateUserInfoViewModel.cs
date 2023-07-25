using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class UpdateUserInfoViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Id { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }
    }
}

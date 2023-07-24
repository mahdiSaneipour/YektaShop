using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Core.DTOs.User
{
    public class LoginUser
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

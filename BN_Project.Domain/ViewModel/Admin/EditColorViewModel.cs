using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class EditColorViewModel
    {
        public int ColorId { get; set; }

        [Display(Name = "اسم رنگ")]
        [Required(ErrorMessage = "پر کردن فیلد {0} اجباری میباشد")]
        public string Name { get; set; }

        [Display(Name = "کد رنگ")]
        [Required(ErrorMessage = "پر کردن فیلد {0} اجباری میباشد")]
        public string Hex { get; set; }

        [Display(Name = "قیمت رنگ")]
        [Required(ErrorMessage = "پر کردن فیلد {0} اجباری میباشد")]
        public long Price { get; set; }

        [Display(Name = "تعدا رنگ")]
        [Required(ErrorMessage = "پر کردن فیلد {0} اجباری میباشد")]
        public int Count { get; set; }

        [Display(Name = "ایدی محصول")]
        [Required(ErrorMessage = "پر کردن فیلد {0} اجباری میباشد")]
        public string ProductName { get; set; }

        public bool IsDefault { get; set; }
    }
}

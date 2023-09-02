using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class AddProductViewModel
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Title { get; set; }

        [Display(Name = "عکس محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Image { get; set; }

        [Display(Name = "ویژگی های محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Feature { get; set; }

        [Display(Name = "دسته بندی محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Category { get; set; }

        [Display(Name = "توضیحات محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Description { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public int Price { get; set; }
    }
}

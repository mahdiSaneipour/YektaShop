using BN_Project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class AddDiscountViewModel
    {
        [Display(Name = "کد تخفیف")]
        public string? Code { get; set; }
        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "تاریخ اتمام")]
        public DateTime? ExpireDate { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Percent { get; set; }
        [Display(Name = "کالا های انتخاب شده")]
        public string SelecetedGoods { get; set; }
        public List<ProductsForDiscountViewModel> Products { get; set; }
    }
}

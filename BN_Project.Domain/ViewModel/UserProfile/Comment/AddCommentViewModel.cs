using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile.Comment
{
    public class AddCommentViewModel
    {
        public string? ProductName { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "کیفیت ساخت")]
        public int BuildQuality { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "ارزش خرید به نسبت قیمت")]
        public int ValueOfPurches { get; set; }
        [Display(Name = "نوآوری")]
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Innovation { get; set; }
        [Display(Name = "امکانات و قابلیت ها")]
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int Fetures { get; set; }
        [Display(Name = "سهولت استفاده")]
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public int EaseOfUse { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "طراحی و ظاهر")]
        public int Apparent { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "عنوان نظر شما")]
        public string Title { get; set; }
        [Display(Name = "نقاط قوت")]
        public string? Strength { get; set; }
        [Display(Name = "نقاط ضعف")]
        public string? WeakPoints { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        [Display(Name = "متن نظر شما")]
        public string CommentMessage { get; set; }
        public int ProductId { get; set; }
        public string? ProductImage { get; set; }
    }
}

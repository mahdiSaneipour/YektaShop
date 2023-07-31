using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class AddGalleryViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string ImageName { get; set; }
        public int ProductId { get; set; }
    }
}

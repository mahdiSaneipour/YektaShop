using BN_Project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.Enum.Ticket
{
    public enum Priority
    {
        [Display(Name = "عادی")]
        Simple,
        [Display(Name = "مهم")]
        Important,
        [Display(Name = "خیلی مهم")]
        VeryImportant
    }
}

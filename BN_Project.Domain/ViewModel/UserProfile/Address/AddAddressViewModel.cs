using System.ComponentModel.DataAnnotations;

namespace BN_Project.Domain.ViewModel.UserProfile.Address
{
    public class AddAddressViewModel
    {
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string Family { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string State { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string City { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "این فیلد ضروری می باشد!")]
        public string CompleteAddress { get; set; }
        public int UserId { get; set; }
    }
}

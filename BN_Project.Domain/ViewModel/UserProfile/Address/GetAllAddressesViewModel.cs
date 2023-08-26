namespace BN_Project.Domain.ViewModel.UserProfile.Address
{
    public class GetAllAddressesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }
}

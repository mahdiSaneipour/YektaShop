namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class PaymentRequestViewModel
    {
        public string MerchentId { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string AdminPhoneNumber { get; set; }
        public string RedirectAddress { get; set; }
    }
}

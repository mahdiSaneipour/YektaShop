namespace BN_Project.Domain.ViewModel.UserProfile.Order
{
    public class FactorCompViewModel
    {
        public int Price { get; set; }
        public int Discount { get; set; }
        public int TotalPrice { get; set; }

        public int Count { get; set; }

        public string DiscountCode { get; set; }

        public int AddressId { get; set; }
    }
}

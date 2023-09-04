namespace BN_Project.Domain.ViewModel.UserProfile.Comment
{
    public class AverageRatingViewModel
    {
        public decimal totalAverageRate { get; set; }
        public decimal totalAveragePercent { get; set; }

        public decimal TotalComments { get; set; }

        public decimal BuildQualityRate { get; set; }
        public decimal BuildQualityPercent { get; set; }

        public decimal ValueOfPurchesRate { get; set; }
        public decimal ValueOfPurchesPercent { get; set; }

        public decimal InnovationRate { get; set; }
        public decimal InnovationPercent { get; set; }

        public decimal FacilityRate { get; set; }
        public decimal FacilityPercent { get; set; }

        public decimal EaseOfUseRate { get; set; }
        public decimal EaseOfUsePercent { get; set; }

        public decimal ApperentRate { get; set; }
        public decimal ApperentPercent { get; set; }

    }
}

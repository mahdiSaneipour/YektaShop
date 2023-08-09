namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Section { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}

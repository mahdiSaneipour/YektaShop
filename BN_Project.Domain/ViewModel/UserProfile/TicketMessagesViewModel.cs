namespace BN_Project.Domain.ViewModel.UserProfile
{
    public class TicketMessagesViewModel
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int OwnerId { get; set; }
        public string Status { get; set; }
        public List<MessagesViewModel> TicketMessages { get; set; }
        public AddMessageViewModel AddMessage { get; set; }
    }
}

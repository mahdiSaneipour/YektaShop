namespace BN_Project.Domain.ViewModel.UserProfile.Comment
{
    public class ShowCommentsForAdminViewModel
    {
        public int CommnetId { get; set; }
        public string? UserName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string? Status { get; set; }
        public List<string> Strength { get; set; }
        public List<string> WeakPoints { get; set; }
        public string CreateDate { get; set; }
    }
}

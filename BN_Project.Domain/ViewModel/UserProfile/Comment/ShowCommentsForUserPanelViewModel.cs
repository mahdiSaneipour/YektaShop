namespace BN_Project.Domain.ViewModel.UserProfile.Comment
{
    public class ShowCommentsForUserPanelViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? UserName { get; set; }
        public string CommentDate { get; set; }
        public string Body { get; set; }
        public List<string>? Strength { get; set; }
        public List<string>? WeakPoints { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public bool LikeOrDisLike { get; set; }
        public bool isThereImpression { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
    }
}

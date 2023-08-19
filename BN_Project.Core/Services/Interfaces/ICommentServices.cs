using BN_Project.Domain.ViewModel.UserProfile.Comment;

namespace BN_Project.Core.Services.Interfaces
{
    public interface ICommentServices
    {
        public Task<AddCommentViewModel> FillProductInformation(int ProductId);
        public Task<bool> InsertComment(AddCommentViewModel comment, int userId);
    }
}

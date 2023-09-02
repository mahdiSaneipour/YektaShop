using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.ViewModel.UserProfile.Comment;
using System.Linq.Expressions;

namespace BN_Project.Domain.IRepository
{
    public interface ICommentRepository : IGenericRepositroy<Comment>
    {
        public Task<List<Comment>> GetCommentsWithRelations(Expression<Func<Comment, bool>> where = null);
        public Task<Comment> GetCommentWithRelations(Expression<Func<Comment, bool>> where);
        public Task<List<CommentRatingsViewModel>> GetAllRatingPoints(int productId);
    }
}

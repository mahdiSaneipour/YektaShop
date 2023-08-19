using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.UserProfile.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Core.Services.Implementations
{
    public class CommentServices : ICommentServices
    {
        private readonly IProductRepository _productServices;
        private readonly ICommentRepository _commentServices;
        public CommentServices(
            IProductRepository productServices,
            ICommentRepository commentServices)
        {
            _productServices = productServices;
            _commentServices = commentServices;
        }

        public async Task<AddCommentViewModel> FillProductInformation(int ProductId)
        {
            var product = await _productServices.GetSingle(n => n.Id == ProductId);
            AddCommentViewModel comment = new AddCommentViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.Image
            };

            return comment;
        }

        public async Task<bool> InsertComment(AddCommentViewModel comment, int userId)
        {
            Comment CommentEntity = new Comment()
            {
                BuildQuality = comment.BuildQuality,
                Innovation = comment.Innovation,
                EaseOfUse = comment.EaseOfUse,
                DesignAndAppearance = comment.Apparent,
                FeaturesAndCapabilities = comment.Fetures,
                ValueForMoneyComparedToTHePrice = comment.ValueOfPurches,
                ProductId = comment.ProductId,
                Title = comment.Title,
                Message = comment.CommentMessage,
                UserId = userId
            };
            if (comment.Strength != null)
            {

            }
            if (comment.WeakPoints != null)
            {

            }

            await _commentServices.Insert(CommentEntity);
            await _commentServices.SaveChanges();

            return true;
        }
    }
}

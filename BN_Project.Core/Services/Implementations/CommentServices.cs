using BN_Project.Core.Services.Interfaces;
using BN_Project.Core.Tools;
using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.UserProfile.Comment;
using Microsoft.AspNetCore.Http;

namespace BN_Project.Core.Services.Implementations
{
    public class CommentServices : ICommentServices
    {
        private readonly IProductRepository _productRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IImpressionsRepository _impressionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentWeakPointsRepository _weakPointsRepository;
        private readonly ICommentStrengthRepository _strengthRepository;
        public CommentServices(
            IProductRepository productRepository,
            ICommentRepository commentRepository,
            IImpressionsRepository impressionRepository,
            IHttpContextAccessor httpContextAccessor,
            ICommentStrengthRepository strengthRepository,
            ICommentWeakPointsRepository weakPointsRepository)
        {
            _productRepository = productRepository;
            _commentRepository = commentRepository;
            _impressionRepository = impressionRepository;
            _httpContextAccessor = httpContextAccessor;
            _strengthRepository = strengthRepository;
            _weakPointsRepository = weakPointsRepository;
        }

        public async Task<AddCommentViewModel> FillProductInformation(int ProductId)
        {
            var product = await _productRepository.GetSingle(n => n.Id == ProductId);
            AddCommentViewModel comment = new AddCommentViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.Image
            };

            return comment;
        }

        public async Task<List<ShowCommentsForUsersViewModel>> GetAllCommentsForUsers()
        {
            List<ShowCommentsForUsersViewModel> comments = new List<ShowCommentsForUsersViewModel>();
            var commentList = await _commentRepository.GetCommentsWithRelations(n => n.IsConfirmed == true);
            int userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);

            foreach (var item in commentList)
            {
                ShowCommentsForUsersViewModel comment = new ShowCommentsForUsersViewModel()
                {
                    Id = item.Id,
                    Body = item.Message,
                    Strength = item.Strengths.Select(n => n.Text).ToList(),
                    WeakPoints = item.WeakPoints.Select(n => n.Text).ToList(),
                    UserName = item.User.Name,
                    CommentDate = item.Create.ConvertToShamsi(),
                    Likes = item.Impressions.Where(n => n.LikeOrDislike == true).Count(),
                    DisLikes = item.Impressions.Where(n => n.LikeOrDislike == false).Count(),
                };

                if (userId != 0)
                {
                    comment.isThereImpression = (await _impressionRepository.IsThereAny(n => n.CommentId == item.Id && n.UserId == userId));
                    if (comment.isThereImpression)
                    {
                        comment.LikeOrDisLike = (await _impressionRepository.IsThereAny(n => n.CommentId == item.Id && n.UserId == userId && n.LikeOrDislike == true)) ? true : false;
                    }
                }
                comments.Add(comment);
            }


            return comments;
        }

        public async Task<AvrageRatingViewModel> GetAllRatingPoints()
        {
            AvrageRatingViewModel avragePoints = new AvrageRatingViewModel();
            var Points = await _commentRepository.GetAllRatingPoints();
            if (Points.Count() != 0)
            {
                avragePoints.TotalComments = Points.Count();
                avragePoints.BuildQualityRate = Points.Select(n => n.BuildQuality).ToList().CalculateAvrage();
                avragePoints.ValueOfPurchesRate = Points.Select(n => n.ValueForMoneyComparedToTHePrice).ToList().CalculateAvrage();
                avragePoints.InnovationRate = Points.Select(n => n.Innovation).ToList().CalculateAvrage();
                avragePoints.FacilityRate = Points.Select(n => n.FeaturesAndCapabilities).ToList().CalculateAvrage();
                avragePoints.EaseOfUseRate = Points.Select(n => n.EaseOfUse).ToList().CalculateAvrage();
                avragePoints.ApperentRate = Points.Select(n => n.DesignAndAppearance).ToList().CalculateAvrage();
                avragePoints.totalAvrageRate = Math.Round((avragePoints.BuildQualityRate + avragePoints.ValueOfPurchesRate + avragePoints.InnovationRate + avragePoints.FacilityRate +
                    avragePoints.EaseOfUseRate + avragePoints.ApperentRate) / 6, 1);

                avragePoints.BuildQualityPercent = avragePoints.BuildQualityRate.CalculateAvragePercent();
                avragePoints.ValueOfPurchesPercent = avragePoints.ValueOfPurchesRate.CalculateAvragePercent();
                avragePoints.InnovationPercent = avragePoints.InnovationRate.CalculateAvragePercent();
                avragePoints.FacilityPercent = avragePoints.FacilityRate.CalculateAvragePercent();
                avragePoints.EaseOfUsePercent = avragePoints.EaseOfUseRate.CalculateAvragePercent();
                avragePoints.ApperentPercent = avragePoints.ApperentRate.CalculateAvragePercent();
                avragePoints.totalAvragePercent = avragePoints.totalAvrageRate.CalculateAvragePercent();

            }
            return avragePoints;
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
                var StrengthList = comment.Strength.Split(",").ToList();
                CommentEntity.Strengths = new List<Strength>();
                foreach (var item in StrengthList)
                {
                    CommentEntity.Strengths.Add(new Strength
                    {
                        Text = item
                    });
                }

            }
            if (comment.WeakPoints != null)
            {
                var WeakPointsList = comment.WeakPoints.Split(",").ToList();
                CommentEntity.WeakPoints = new List<WeakPoint>();
                foreach (var item in WeakPointsList)
                {
                    CommentEntity.WeakPoints.Add(new WeakPoint
                    {
                        Text = item
                    });
                }
            }

            await _commentRepository.Insert(CommentEntity);
            await _commentRepository.SaveChanges();

            return true;
        }

        public async Task<bool> LikeComment(int commentId, int userId)
        {
            if (await _impressionRepository.IsThereAny(n => n.UserId == userId && n.CommentId == commentId))
            {
                var impression = await _impressionRepository.GetSingle(n => n.UserId == userId && n.CommentId == commentId);
                if (impression.LikeOrDislike == true)
                {
                    _impressionRepository.Delete(impression);
                    await _impressionRepository.SaveChanges();
                    return true;
                }
                impression.LikeOrDislike = true;
            }
            else
            {
                Impression impression = new Impression()
                {
                    CommentId = commentId,
                    LikeOrDislike = true,
                    UserId = userId
                };
                await _impressionRepository.Insert(impression);
            }
            await _impressionRepository.SaveChanges();

            return true;
        }

        public async Task<bool> DisLikeComment(int commentId, int userId)
        {
            if (await _impressionRepository.IsThereAny(n => n.UserId == userId && n.CommentId == commentId))
            {
                var impression = await _impressionRepository.GetSingle(n => n.UserId == userId && n.CommentId == commentId);
                if (impression.LikeOrDislike == false)
                {
                    _impressionRepository.Delete(impression);
                    await _impressionRepository.SaveChanges();
                    return true;
                }
                impression.LikeOrDislike = false;
            }
            else
            {
                Impression impression = new Impression()
                {
                    CommentId = commentId,
                    LikeOrDislike = false,
                    UserId = userId
                };
                await _impressionRepository.Insert(impression);
            }
            await _impressionRepository.SaveChanges();

            return true;
        }

        public async Task<List<ShowCommentsForAdminViewModel>> GetAllCommentsForAdmin()
        {
            List<ShowCommentsForAdminViewModel> comments = new List<ShowCommentsForAdminViewModel>();
            var commentList = await _commentRepository.GetCommentsWithRelations();

            foreach (var item in commentList)
            {
                ShowCommentsForAdminViewModel comment = new ShowCommentsForAdminViewModel()
                {
                    CommnetId = item.Id,
                    Title = item.Title,
                    Body = item.Message,
                    Strength = item.Strengths.Select(n => n.Text).ToList(),
                    WeakPoints = item.WeakPoints.Select(n => n.Text).ToList(),
                    UserName = item.User.Name,
                    CreateDate = item.Create.ConvertToShamsi()
                };
                switch (item.IsConfirmed)
                {
                    case true:
                        comment.Status = "تایید شده";
                        break;
                    case false:
                        comment.Status = "رد شده";
                        break;
                    case null:
                        comment.Status = "بررسی نشده";
                        break;
                }
                comments.Add(comment);
            }

            return comments;
        }

        public async Task ConfirmComment(int commentId)
        {
            var comment = await _commentRepository.GetSingle(n => n.Id == commentId);
            comment.IsConfirmed = true;
            _commentRepository.Update(comment);

            await _commentRepository.SaveChanges();
        }

        public async Task CloseComment(int commentId)
        {
            var comment = await _commentRepository.GetSingle(n => n.Id == commentId);
            comment.IsConfirmed = false;
            _commentRepository.Update(comment);

            await _commentRepository.SaveChanges();
        }

        public async Task<List<ShowCommentsForUserPanelViewModel>> GetAllCommentsForUserPanel()
        {
            List<ShowCommentsForUserPanelViewModel> comments = new List<ShowCommentsForUserPanelViewModel>();
            var commentList = await _commentRepository.GetCommentsWithRelations(n => n.IsConfirmed == true);
            int userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);

            foreach (var item in commentList)
            {
                ShowCommentsForUserPanelViewModel comment = new ShowCommentsForUserPanelViewModel()
                {
                    Id = item.Id,
                    Body = item.Message,
                    Strength = item.Strengths.Select(n => n.Text).ToList(),
                    WeakPoints = item.WeakPoints.Select(n => n.Text).ToList(),
                    UserName = item.User.Name,
                    CommentDate = item.Create.ConvertToShamsi(),
                    Likes = item.Impressions.Where(n => n.LikeOrDislike == true).Count(),
                    DisLikes = item.Impressions.Where(n => n.LikeOrDislike == false).Count(),
                    ProductImage = item.Product.Image,
                    ProductName = item.Product.Name
                };

                if (userId != 0)
                {
                    comment.isThereImpression = (await _impressionRepository.IsThereAny(n => n.CommentId == item.Id && n.UserId == userId));
                    if (comment.isThereImpression)
                    {
                        comment.LikeOrDisLike = (await _impressionRepository.IsThereAny(n => n.CommentId == item.Id && n.UserId == userId && n.LikeOrDislike == true)) ? true : false;
                    }
                }
                comments.Add(comment);
            }


            return comments;
        }

        public async Task DeleteCommentByUser(int Id)
        {
            var impressions = await _impressionRepository.GetAll(n => n.CommentId == Id);
            foreach (var item in impressions)
            {
                _impressionRepository.Delete(item);
            }
            await _impressionRepository.SaveChanges();

            var weakPoints = await _weakPointsRepository.GetAll();
            foreach (var item in weakPoints)
            {
                _weakPointsRepository.Delete(item);
            }
            await _weakPointsRepository.SaveChanges();

            var strengths = await _strengthRepository.GetAll();
            foreach (var item in strengths)
            {
                _strengthRepository.Delete(item);
            }
            await _strengthRepository.SaveChanges();


            var comment = await _commentRepository.GetSingle(n => n.Id == Id);
            _commentRepository.Delete(comment);

            await _commentRepository.SaveChanges();
        }
    }
}

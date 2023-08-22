﻿using BN_Project.Domain.ViewModel.UserProfile.Comment;

namespace BN_Project.Core.Services.Interfaces
{
    public interface ICommentServices
    {
        public Task<AddCommentViewModel> FillProductInformation(int ProductId);
        public Task<bool> InsertComment(AddCommentViewModel comment, int userId);
        public Task<AvrageRatingViewModel> GetAllRatingPoints();
        public Task<List<ShowCommentsForUsersViewModel>> GetAllCommentsForUsers();
        public Task<bool> LikeComment(int commentId, int userId);
        public Task<bool> DisLikeComment(int commentId, int userId);
        public Task<List<ShowCommentsForAdminViewModel>> GetAllCommentsForAdmin();
        public Task ConfirmComment(int commentId);
        public Task CloseComment(int commentId);
        public Task<List<ShowCommentsForUserPanelViewModel>> GetAllCommentsForUserPanel();
        public Task DeleteCommentByUser(int Id);
    }
}
using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Components.Comments
{
    public class CommentsRatingViewComponent : ViewComponent
    {
        private readonly ICommentServices _commentServices;
        public CommentsRatingViewComponent(ICommentServices commentServices)
        {
            _commentServices = commentServices;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var rate = await _commentServices.GetAllRatingPoints(productId);
            return View("CommentsRating", rate);
        }
    }
}

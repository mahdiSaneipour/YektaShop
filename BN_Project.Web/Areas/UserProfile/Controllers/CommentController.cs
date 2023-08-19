using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.UserProfile.Controllers
{
    [Authorize]
    [Area("UserProfile")]
    [Route("[Controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentServices _commentServices;
        public CommentController(ICommentServices commentServices)
        {
            _commentServices = commentServices;
        }

        [NonAction]
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        [Route("AddComment")]
        public async Task<IActionResult> AddComment(int Id)
        {
            if (Id == 0)
                return NotFound();
            var item = await _commentServices.FillProductInformation(Id);
            return View(item);
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment(AddCommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                var item = await _commentServices.FillProductInformation(comment.ProductId);
                comment.ProductImage = item.ProductImage;
                comment.ProductName = item.ProductName;
                return View(comment);
            }

            int UserId = GetCurrentUserId();

            if (await _commentServices.InsertComment(comment, UserId))
            {
                return RedirectToAction("ShowProduct", "Home", new { area = "", productId = comment.ProductId });
            }
            else
            {
                return NotFound();
            }
        }
    }
}

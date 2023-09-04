using BN_Project.Core.Attributes;
using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Admin.Controllers
{
    [PermissionCheker("Comments_Management")]
    [Area("Admin")]
    [Route("[Controller]")]

    public class CommentsController : Controller
    {
        private readonly ICommentServices _commentServices;
        public CommentsController(ICommentServices commentServices)
        {
            _commentServices = commentServices;
        }
        [PermissionCheker("Comments_Comments")]
        [Route("Comments")]
        public async Task<IActionResult> Comments()
        {
            var comments = await _commentServices.GetAllCommentsForAdmin();
            return View(comments);
        }

        [PermissionCheker("ConfirmComment_Comment")]
        [Route("ConfirmComment")]
        public async Task<IActionResult> ConfirmComment(int Id)
        {
            await _commentServices.ConfirmComment(Id);
            return RedirectToAction(nameof(Comments));
        }

        [PermissionCheker("CloseComment_Comment")]
        public async Task<IActionResult> CloseComment(int Id)
        {
            await _commentServices.CloseComment(Id);

            return RedirectToAction(nameof(Comments));
        }
    }
}

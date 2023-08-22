using BN_Project.Core.Enums.Admin;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICommentServices _commentServices;

        public HomeController(IProductServices productServices,
            ICommentServices commentServices)
        {
            _productServices = productServices;
            _commentServices = commentServices;
        }

        [NonAction]
        private int GetCurrentUserId()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            return UserId;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductsGroup(int categoryId, OrderByEnum? orderBy = OrderByEnum.Newest)
        {
            var result = await _productServices.GetProductsListShowByCategoryId(categoryId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return Redirect("/");
        }

        public async Task<IActionResult> ShowProduct(int productId)
        {
            var result = await _productServices.GetProductForShowByProductId(productId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return Redirect("/");
        }

        public async Task<IActionResult> LikeComment(int commentId)
        {
            int userId = GetCurrentUserId();
            await _commentServices.LikeComment(commentId, userId);
            return ViewComponent("Comments");
        }

        public async Task<IActionResult> DisLikeComment(int commentId)
        {
            int userId = GetCurrentUserId();
            await _commentServices.DisLikeComment(commentId, userId);
            return ViewComponent("Comments");
        }
    }
}

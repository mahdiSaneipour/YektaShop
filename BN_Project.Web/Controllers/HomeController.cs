using BN_Project.Core.Enums.Admin;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServices _productServices;

        public HomeController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ProductsGroup/{categoryId}/{orderBy?}")]
        public async Task<IActionResult> ProductsGroup(int categoryId, OrderByEnum? orderBy = OrderByEnum.Newest)
        {
            var result = await _productServices.GetProductsListShowByCategoryId(categoryId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return Redirect("/");
        }

        [HttpGet]
        [Route("ShowProduct/{productId}")]
        public async Task<IActionResult> ShowProduct(int productId)
        {
            var result = await _productServices.GetProductForShowByProductId(productId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return Redirect("/");
        }
    }
}

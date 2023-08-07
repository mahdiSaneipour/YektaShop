using BN_Project.Core.Enums.Admin;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("[Controller]")]
    public class Products : Controller
    {
        private readonly IProductServices _productServices;

        public Products(IProductServices productServices)
        {
            _productServices = productServices;
        }

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
    }
}

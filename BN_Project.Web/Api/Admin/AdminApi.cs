using BN_Project.Core.Services.Interfaces;
using BN_Project.Core.Tools.Image;
using BN_Project.Core.Tools.ImageResizer;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Api.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApi : ControllerBase
    {
        private readonly IProductServices _productServices;

        public AdminApi(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        [Route("UploadAvatarImage")]
        [Produces("application/json")]
        public IActionResult UploadAvatarImage(IFormFile file)
        {
            string name = UploadImage.UploadFileImage(file, "wwwroot/images/avatar/normal");
            UploadImage.UploadFileImage(file, "wwwroot/images/avatar/thumb", name);

            var resizer = new ImageConvertor();

            string fullNormalPath = Directory.GetCurrentDirectory() + "/wwwroot/images/avatar/normal/" + name;
            string fullThumbPath = Directory.GetCurrentDirectory() + "/wwwroot/images/avatar/thumb/" + name;

            resizer.Image_resize(fullNormalPath, fullThumbPath, 200);

            return Ok(new { name = name });
        }

        [HttpPost]
        [Route("UploadProductImage")]
        [Produces("application/json")]
        public IActionResult UploadProductImage(IFormFile file)
        {
            string name = UploadImage.UploadFileImage(file, "wwwroot/images/products/normal");
            UploadImage.UploadFileImage(file, "wwwroot/images/products/thumb", name);

            var resizer = new ImageConvertor();

            string fullNormalPath = Directory.GetCurrentDirectory() + "/wwwroot/images/products/normal/" + name;
            string fullThumbPath = Directory.GetCurrentDirectory() + "/wwwroot/images/products/thumb/" + name;

            resizer.Image_resize(fullNormalPath, fullThumbPath, 200);

            return Ok(new { name = name });
        }

        [HttpPost]
        [Route("UploadGalleryImage")]
        [Produces("application/json")]
        public IActionResult UploadGalleryImage(IFormFile file)
        {
            string name = UploadImage.UploadFileImage(file, "wwwroot/images/gallery/normal");
            UploadImage.UploadFileImage(file, "wwwroot/images/gallery/thumb", name);

            var resizer = new ImageConvertor();

            string fullNormalPath = Directory.GetCurrentDirectory() + "/wwwroot/images/gallery/normal/" + name;
            string fullThumbPath = Directory.GetCurrentDirectory() + "/wwwroot/images/gallery/thumb/" + name;

            resizer.Image_resize(fullNormalPath, fullThumbPath, 200);

            return Ok(new { name = name });
        }

        [HttpGet]
        [Route("GetSubCategoryByCategoryId/{categoryId}")]
        [Produces("application/json")]
        public IActionResult GetSubCategoryByCategoryId(int categoryId)
        {
            return Ok(_productServices.GetSubCategories(categoryId).Result);
        }

        [HttpGet]
        [Route("SearchProduct")]
        [Produces("application/json")]
        public IActionResult SearchProduct()
        {
            try
            {
                string filter = HttpContext.Request.Query["term"].ToString();
                List<string> result = _productServices.SearchProductByName(filter).Result.Data;

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

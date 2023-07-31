using BN_Project.Core.IService.Admin;
using BN_Project.Core.Tools.Image;
using BN_Project.Core.Tools.ImageResizer;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Api.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApi : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminApi(IAdminServices adminServices)
        {
            _adminServices = adminServices;
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

        [HttpGet]
        [Route("GetSubCategoryByCategoryId/{categoryId}")]
        [Produces("application/json")]
        public IActionResult GetSubCategoryByCategoryId(int categoryId)
        {
            return Ok(_adminServices.GetSubCategories(categoryId).Result);
        }

        [HttpGet]
        [Route("DeleteProductByProductId/{productId}")]
        [Produces("application/json")]
        public IActionResult DeleteProductByProductId(int productId)
        {
            return Ok(_adminServices.DeleteProductByProductId(productId));
        }
    }
}

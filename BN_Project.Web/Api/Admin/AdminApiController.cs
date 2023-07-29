using BN_Project.Core.Tools.Image;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Api.Admin
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        [HttpPost]
        [Route("UploadAvatar")]
        [Produces("application/json")]
        public IActionResult UploadProductImage(IFormFile file)
        {
            string name = UploadImage.UploadFileImage(file, "wwwroot/images/avatar/normal");
            UploadImage.UploadFileImage(file, "wwwroot/images/avatar/thumb", name);

            var resizer = new Core.Tools.ImageResizer.ImageConvertor();

            string fullNormalPath = Directory.GetCurrentDirectory() + "/wwwroot/images/avatar/normal/" + name;
            string fullThumbPath = Directory.GetCurrentDirectory() + "/wwwroot/images/avatar/thumb/" + name;

            resizer.Image_resize(fullNormalPath, fullThumbPath, 200);

            return Ok(new { name = name });
        }
    }
}

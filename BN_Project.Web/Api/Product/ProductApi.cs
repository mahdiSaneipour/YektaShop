using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Api.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApi : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductApi(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        [Route("GetPriceByColorId/{colorId}")]
        public async Task<IActionResult> GetPriceByColorId(int colorId)
        {
            int price = await _productServices.GetPriceByColorId(colorId);

            return Ok(new { price = price });
        }

        [HttpGet]
        [Route("GetCountByColorId/{colorId}")]
        public async Task<IActionResult> GetCountByColorId(int colorId)
        {
            long count = await _productServices.GetCountByColorId(colorId);

            return Ok(new { count = count });
        }
    }
}

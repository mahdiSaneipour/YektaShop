using BN_Project.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Components.Admin
{
    public class MenuCategoriesViewComponent : ViewComponent
    {
        private readonly IProductServices _productServices;

        public MenuCategoriesViewComponent(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _productServices.GetAllCategoriesForHeader());
        }
    }
}

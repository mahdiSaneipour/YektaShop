using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.UserProfile.Order;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Components.Order
{
    public class FactorViewComponent : ViewComponent
    {
        private readonly IOrderServices _orderServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FactorViewComponent(IOrderServices orderServices, IHttpContextAccessor httpContextAccessor)
        {
            _orderServices = orderServices;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IViewComponentResult> InvokeAsync(int AddressId = 0)
        {
            FactorCompViewModel data = new FactorCompViewModel();

            int userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value.ToString());

            var result = await _orderServices.GetFactorCompModel(userId);

            data = result.Data;

            if (AddressId != 0)
                data.AddressId = AddressId;

            return View("Factor", data);
        }
    }
}
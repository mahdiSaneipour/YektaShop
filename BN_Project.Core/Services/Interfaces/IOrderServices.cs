using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.ViewModel.UserProfile.Order;

namespace BN_Project.Core.Services.Interfaces
{
    public interface IOrderServices
    {
        public Task<DataResponse<List<BoxOrderListViewModel>>> GetBoxOrderList(OrderStatus orderStatus, int userId);

        public Task<DataResponse<List<BoxBasketListViewModel>>> GetBasketOrders(int userId);

        public Task<BaseResponse> ChangeProductOrderCount(int orderDetailId, int count = 1);

        public Task<BaseResponse> DeleteOrderDetailByOrderDetailId(int orderDetailId);

        public Task<BaseResponse> AddProductToBasket(int colorId);

        public Task<BaseResponse> SetPricesInOrderDetail(int orderDetailId);

        public Task<int> GetFinalPriceForBasket(int userId);

        public Task<DataResponse<FactorCompViewModel>> GetFactorCompModel(int userId);

        public Task<BaseResponse> SetPriceInOrder(int orderId);

        public Task<BaseResponse> ApplyDiscount(string discount);

        public Task CheckOrderDetailsInBasket();
    }
}

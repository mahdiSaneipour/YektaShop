using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.UserProfile.Order;

namespace BN_Project.Core.Services.Interfaces
{
    public interface IOrderServices
    {
        public Task<DataResponse<List<BoxOrderListViewModel>>> GetBoxOrderList(OrderStatus orderStatus, int userId);

        public Task<DataResponse<List<BoxBasketListViewModel>>> GetBasketOrders(int userId);

        public Task<BaseResponse> ChangeProductOrderCount(int orderDetailId, bool status);

        public Task<BaseResponse> DeleteOrderDetailByOrderDetailId(int orderDetailId);

        public Task<BaseResponse> AddProductToBasket(int colorId, int userId);

        public Task<BaseResponse> SetPricesInOrderDetail(int orderDetailId);
    }
}

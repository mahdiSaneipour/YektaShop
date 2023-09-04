using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.ViewModel.Admin;

namespace BN_Project.Domain.IRepository
{
    public interface IOrderRepository : IGenericRepositroy<Order>
    {
        public Task<List<Order>> GetOrderBoxByStatusWithIncludeOrderDetail(OrderStatus orderStatus, int userId);

        public Task<List<string>> GetProductImagesByOrderId(int orderId);

        public Task<Order> GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(int userId);

        public Task<Order> GetOrderWithIncludeOrderDetail(int orderId);

        public Task<int> GetBasketIdByUserId(int userId);

        public Task<Order> GetBasketByIdByIncludes(int basketId);

        public Task<List<ChartDataViewModel>> GetChartDataForMostSellsInPast10Days();

        public Task<List<ChartDataViewModel>> GetChartDataForMostSellsThisMonth();

        public Task<List<ChartDataViewModel>> GetChartDataForMost5PopularProduct();

    }
}

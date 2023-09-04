using BN_Project.Core.Tools;
using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BN_Project.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly BNContext _context;

        public OrderRepository(BNContext context) : base (context)
        {
            _context = context;
        }

        public async Task<int> GetBasketIdByUserId(int userId)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Status == 0);

            return result.Id;
        }

        public async Task<Order> GetBasketByIdByIncludes(int basketId)
        {
            return await _context.Orders.Where(o => o.Id == basketId)
                .Include(o => o.Discount).Include(o => o.OrderDetails)
                .ThenInclude(od => od.Color).ThenInclude(c => c.Product)
                .ThenInclude(p => p.DiscountProduct).AsSplitQuery().FirstOrDefaultAsync();
        }

        public async Task<Order> GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId && o.Status == 0)
                .Include(o => o.Discount).Include(o => o.OrderDetails)
                .ThenInclude(od => od.Color).ThenInclude(c => c.Product)
                .ThenInclude(p => p.DiscountProduct).AsSplitQuery().FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrderBoxByStatusWithIncludeOrderDetail(OrderStatus orderStatus, int userId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).Where(o => o.Status == orderStatus && o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderWithIncludeOrderDetail(int orderId)
        {
            return await _context.Orders.Include(c => c.OrderDetails).Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetProductImagesByOrderId(int orderId)
        {
            var result = _context.OrderDetails.Include(od => od.Color)
                .ThenInclude(c => c.Product).Where(o => o.OrderId == orderId);

            return await result.Select(od => od.Color.Product.Image).ToListAsync();
        }

        public async Task<List<ChartDataViewModel>> GetChartDataForMostSellsInPast10Days()
        {
            List<ChartDataViewModel> result = new List<ChartDataViewModel>();

            result = await _context.OrderDetails.Include(od => od.Order).Include(od => od.Color)
                .ThenInclude(c => c.Product)
                .Where(od => od.Order.Status == OrderStatus.Processing
                && od.Order.Create >= DateTime.Now.AddDays(-10)).GroupBy(od => od.ColorId)
                .Select(od => new ChartDataViewModel()
                {
                    Count = od.Sum(c => c.Count),
                    Title = od.Key.ToString()
                }).OrderByDescending(od => od.Count).Take(3).ToListAsync();

            for(var i = 0; i < result.Count; i++)
            {
                result[i].Title = await GetFullNameWithColorIdd(Int32.Parse(result[i].Title));
            }

            result.Add(new ChartDataViewModel()
            {
                Count = _context.OrderDetails.Where(od => od.Order.Status == OrderStatus.Processing
                && od.Order.Create >= DateTime.Now.AddDays(-10)).Sum(c => c.Count) - result.Sum(c => c.Count),
                Title = "سایر محصولات"
            });

            return result;
        }

        public async Task<List<ChartDataViewModel>> GetChartDataForMostSellsThisMonth()
        {
            List<ChartDataViewModel> result = new List<ChartDataViewModel>();

            result = await _context.OrderDetails.Include(od => od.Order).Include(od => od.Color)
                .ThenInclude(c => c.Product)
                .Where(od => od.Order.Status == OrderStatus.Processing
                && od.Order.Create >= Tools.GetStartOfMonth() && od.Order.Create < Tools.GetEndOfMonth()).GroupBy(od => od.ColorId)
                .Select(od => new ChartDataViewModel()
                {
                    Count = od.Sum(c => c.Count),
                    Title = od.Key.ToString()
                }).OrderByDescending(od => od.Count).Take(3).ToListAsync();

            for (var i = 0; i < result.Count; i++)
            {
                result[i].Title = await GetFullNameWithColorIdd(Int32.Parse(result[i].Title));
            }

            result.Add(new ChartDataViewModel()
            {
                Count = _context.OrderDetails.Where(od => od.Order.Status == OrderStatus.Processing
                && od.Order.Create >= Tools.GetStartOfMonth() && od.Order.Create < Tools.GetEndOfMonth()).Sum(c => c.Count) - result.Sum(c => c.Count),
                Title = "سایر محصولات"
            });

            return result;
        }

        public async Task<List<ChartDataViewModel>> GetChartDataForMost5PopularProduct()
        {
            List<ChartDataViewModel> result = new List<ChartDataViewModel>();

            result = await _context.OrderDetails.Include(od => od.Order).Include(od => od.Color)
                .ThenInclude(c => c.Product)
                .Where(od => od.Order.Status == OrderStatus.Processing).GroupBy(od => od.ColorId)
                .Select(od => new ChartDataViewModel()
                {
                    Count = od.Sum(c => c.Count),
                    Title = od.Key.ToString()
                }).OrderByDescending(od => od.Count).Take(5).ToListAsync();

            int countAll = _context.OrderDetails.Include(od => od.Order)
                    .Where(od => od.Order.Status == OrderStatus.Processing)
                    .Sum(od => od.Count);

            for (var i = 0; i < result.Count; i++)
            {
                result[i].Count = Math.Round(Tools.HowManyPercentOfMain(countAll, (int)result[i].Count));
                result[i].Title = await GetFullNameWithColorIdd(Int32.Parse(result[i].Title));
            }

            result.Add(new ChartDataViewModel()
            {
                Count = 100 - result.Sum(c => c.Count),
                Title = "سایر محصولات"
            });;

            return result;
        }

        private async Task<string> GetFullNameWithColorIdd(int colorIdId)
        {
            string result = await _context.Colors.Where(c => c.Id == colorIdId)
                .Include(c => c.Product)
                .Select(d => d.Product.Name + " - " + d.Name)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}

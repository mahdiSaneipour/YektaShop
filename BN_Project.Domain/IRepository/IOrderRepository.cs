using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.ViewModel.UserProfile.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetOrderBoxByStatusWithIncludeOrderDetail(OrderStatus orderStatus, int userId);

        public Task<List<string>> GetProductImagesByOrderId(int orderId);
    }
}

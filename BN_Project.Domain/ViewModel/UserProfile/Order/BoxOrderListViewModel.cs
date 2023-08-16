using BN_Project.Domain.Enum.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.UserProfile.Order
{
    public class BoxOrderListViewModel
    {
        public int OrderId { get; set; }

        public DateTime CreateDate { get; set; }

        public long FinalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public List<string> ProductImages { get; set; }
    }
}

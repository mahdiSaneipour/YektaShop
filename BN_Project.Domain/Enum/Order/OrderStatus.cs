using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Enum.Order
{
    public enum OrderStatus
    {
        AwaitingPayment,
        Processing,
        Delivered,
        Returned,
        Canceled
    }
}

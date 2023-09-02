using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities.OrderBasket
{
    public class PurchesHistory : BaseEntity
    {
        public string Authority { get; set; }
        public int OrderId { get; set; }
        public string RefId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}

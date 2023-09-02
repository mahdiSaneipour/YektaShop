using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int Count { get; set; }

        public int Price { get; set; }

        public int FinalPrice { get; set; }

        public int ColorId { get; set; }

        public int OrderId { get; set; }

        public DateTime ExpireTime { get; set; } = DateTime.Now.AddDays(2);

        [ForeignKey(nameof(ColorId))]
        public Color Color { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}

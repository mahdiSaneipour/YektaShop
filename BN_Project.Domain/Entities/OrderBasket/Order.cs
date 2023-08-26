using BN_Project.Domain.Entities.Common;
using BN_Project.Domain.Enum.Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int AddressId { get; set; }
        public long FinalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Discount { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public int UserId { get; set; }

        #region Relations
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
        #endregion
    }
}
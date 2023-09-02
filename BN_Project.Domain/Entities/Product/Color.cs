using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }

        public string Hex { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        public bool IsDefault { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

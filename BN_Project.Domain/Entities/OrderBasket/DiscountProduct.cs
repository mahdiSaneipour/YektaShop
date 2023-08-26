using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class DiscountProduct : BaseEntity
    {
        public int DiscountsId { get; set; }

        [ForeignKey("DiscountsId")]
        public Discount? Discount { get; set; }

        public int ProductsId { get; set; }
        [ForeignKey("ProductsId")]

        public Product? Product { get; set; }
    }
}

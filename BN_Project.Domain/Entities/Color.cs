using BN_Project.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }

        public string Hex { get; set; }

        public int Count { get; set; }

        public long Price { get; set; }

        public bool IsDefault { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

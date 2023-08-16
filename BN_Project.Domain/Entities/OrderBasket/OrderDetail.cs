using BN_Project.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int Count { get; set; }

        public long Price { get; set; }

        public long FinalPrice { get; set; }

        public int ColorId { get; set; }

        public int OrderId { get; set; }

        [ForeignKey(nameof(ColorId))]
        public Color Color { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}

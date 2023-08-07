using BN_Project.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities
{
    public class DiscountProduct : BaseEntity
    {
        public int DiscountsId { get; set; }
        public int ProductsId { get; set; }
    }
}

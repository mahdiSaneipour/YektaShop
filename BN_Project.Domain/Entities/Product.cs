using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Features { get; set; }

        public string Description { get; set; }

        public long Price { get; set; }

        public int CategoryId { get; set; }


    }
}

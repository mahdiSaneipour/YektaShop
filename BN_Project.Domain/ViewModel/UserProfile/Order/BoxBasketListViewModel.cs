using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.UserProfile.Order
{
    public class BoxBasketListViewModel
    {
        public int OrderDetailId { get; set; }

        public int ProductId { get; set; }

        public int ColorId { get; set; }

        public string ColorName { get; set; }

        public string Hex { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public string Image { get; set; }

        public int Count { get; set; }
    }
}

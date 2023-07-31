using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.ViewModel.Admin
{
    public class ListColorViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Hex { get; set; }

        public long Price { get; set; }

        public int Count { get; set; }

        public bool IsDefault { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }
    }
}

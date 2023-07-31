using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime Create { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;
    }
}

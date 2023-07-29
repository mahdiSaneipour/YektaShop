﻿using BN_Project.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Image { get; set; }

        public string Features { get; set; }

        public string Description { get; set; }

        public long Price { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}

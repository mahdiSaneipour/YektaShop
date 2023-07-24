using BN_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Data.Context
{
    public class BNContext : DbContext
    {
        public BNContext(DbContextOptions<BNContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}

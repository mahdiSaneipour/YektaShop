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

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasQueryFilter(u => !u.IsDelete);
            base.OnModelCreating(modelBuilder);
        }
    }
}

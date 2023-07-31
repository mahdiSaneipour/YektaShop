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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserEntity>().HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Category>().HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Product>().HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Color>().HasQueryFilter(u => !u.IsDelete);


            var cascadeFKs = modelBuilder
                            .Model
                            .GetEntityTypes()
                            .SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Category>()
                .HasOne(n => n.ParentCategory)
                .WithMany(n => n.SubCategories)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }
    }
}

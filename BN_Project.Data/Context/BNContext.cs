using BN_Project.Domain.Entities;
using BN_Project.Domain.Entities.Authentication;
using BN_Project.Domain.Entities.Comment;
using BN_Project.Domain.Entities.OrderBasket;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Context
{
    public class BNContext : DbContext
    {
        public BNContext(DbContextOptions<BNContext> options)
            : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGallery> ProductGallery { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountProduct> DiscountProduct { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessages> TicketMessages { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Strength> Strengths { get; set; }
        public DbSet<WeakPoint> WeakPoints { get; set; }
        public DbSet<Impression> Impressions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PurchesHistory> PurchesHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UsersRoles> UsersRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolesPermissions> RolesPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BNContext).Assembly);

            modelBuilder.Entity<UserEntity>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Category>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Product>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Color>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Discount>().HasQueryFilter(n => !n.IsDelete);
            modelBuilder.Entity<DiscountProduct>().HasQueryFilter(n => !n.IsDelete);
            modelBuilder.Entity<ProductGallery>().HasQueryFilter(n => !n.IsDelete);
            modelBuilder.Entity<Order>().HasQueryFilter(n => !n.IsDelete);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(n => !n.IsDelete);
            modelBuilder.Entity<Address>().HasQueryFilter(n => !n.IsDelete);

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

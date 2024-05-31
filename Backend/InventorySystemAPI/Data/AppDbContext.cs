using InventorySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<Inventory>? Inventory { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductCategory>? ProductCategories { get; set; }
        public DbSet<ProductSupplier>? ProductSuppliers { get; set; }
        public DbSet<Supplier>? Suppliers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.OrderType)
                .HasConversion<string>();

                entity.ToTable(t => t.HasCheckConstraint("CHK_Orders_FkCustomerID",
            @"(OrderType = 'Sale' AND FkCustomerID IS NOT NULL AND FkSupplierID IS NULL) OR
              (OrderType = 'Purchase' AND FkSupplierID IS NOT NULL AND FkCustomerID IS NULL)"));
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(c => c.Email)
                .IsUnique();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasIndex(pc => pc.ProductCategoryName)
                .IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.ProductName).IsUnique();
                entity.Property(p => p.SellPrice).HasColumnType("decimal(10,2)");
                entity.Property(p => p.CostPrice).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Inventory)
                .WithOne(i => i.Products)
                .HasForeignKey<Inventory>(i => i.FkProductId);

            });

        }
    }
}

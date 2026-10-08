using CartServices.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartServices.DAL.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.CartId).ValueGeneratedOnAdd();

                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.CustomerAddress).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreationDate).IsRequired();
                entity.Property(e => e.LastUpdateDate).IsRequired();

                entity.HasIndex(e => e.CustomerId);

                entity.HasMany(e => e.CartItems)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.CartItemId);
                entity.Property(e => e.CartItemId).ValueGeneratedOnAdd();

                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasIndex(e => new { e.CartId, e.ProductId }).IsUnique();

                entity.HasCheckConstraint("CK_CartItems_Quantity", "Quantity > 0");
                entity.HasCheckConstraint("CK_CartItems_UnitPrice", "UnitPrice >= 0");
            });
        }
    }
}

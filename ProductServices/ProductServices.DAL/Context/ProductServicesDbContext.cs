using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductServices.DAL.Models;

namespace ProductServices.DAL.Context;

public partial class ProductServicesDbContext : DbContext
{
    public ProductServicesDbContext()
    {
    }

    public ProductServicesDbContext(DbContextOptions<ProductServicesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailabilityStatus> AvailabilityStatuses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryStatus> CategoryStatuses { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailabilityStatus>(entity =>
        {
            entity.HasKey(e => e.AvailabilityStatusId).HasName("PK__Availabi__611FF0967958E8FE");

            entity.HasIndex(e => e.Name, "UQ__Availabi__737584F686D7B416").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B26C43059");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F61268BFA0").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Status).WithMany(p => p.Categories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_CategoryStatuses");
        });

        modelBuilder.Entity<CategoryStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Category__C8EE2063AFB8DE0E");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F67292B709").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Inventor__B40CC6CDB834D94F");

            entity.ToTable("Inventory");

            entity.Property(e => e.ProductId).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD208FD5BB");

            entity.HasIndex(e => e.ProductCode, "UQ__Products__2F4E024FD0029055").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.AvailabilityStatus).WithMany(p => p.Products)
                .HasForeignKey(d => d.AvailabilityStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_AvailabilityStatuses");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductCategories_Categories"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductCategories_Products"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("PK__ProductC__159C556DC15F1426");
                        j.ToTable("ProductCategories");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

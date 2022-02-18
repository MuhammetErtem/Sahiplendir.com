using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Animal>(entity =>
            {
                entity
               .HasIndex(p => new { p.Name })
               .IsUnique(true);

                entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);


                entity
               .Property(p => p.Image)
               .IsRequired()
               .IsUnicode(false);
            });
            builder.Entity<Banner>(entity =>
            {
                entity
               .Property(p => p.Image)
               .IsRequired()
               .IsUnicode(false);
            });
            builder.Entity<Brand>(entity => {

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique();

                entity
               .Property(p => p.Image)
               .IsRequired()
               .IsUnicode(false);

                entity
               .Property(p => p.Name)
               .HasMaxLength(50) //max450 verilebilir.
               .IsRequired();

                entity
                .HasMany(p => p.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Category>(entity => {

                entity
                .HasIndex(p => new { p.Name, p.RayonId })
                .IsUnique(true);

                entity
               .Property(p => p.Name)
               .HasMaxLength(50)
               .IsRequired();

                entity
                .HasMany(p => p.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Order>(entity => {

                entity
                .HasIndex(p => new { p.DateCreated });

                entity
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<OrderItem>(entity => {

                entity
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            });

            builder.Entity<Product>(entity => {

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

                entity
                .Property(p => p.Price)
                .HasPrecision(18, 4);

                entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

                entity
                .HasMany(p => p.OrderItem)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            });

            builder.Entity<Rayon>(entity => {

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

                entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

                entity
                .HasMany(p => p.Categories)
                .WithOne(p => p.Rayon)
                .HasForeignKey(p => p.RayonId)
                .OnDelete(DeleteBehavior.Restrict);

                //Restrict o veriyi silip silmemenize yardımcı olur

            });


            base.OnModelCreating(builder);
        }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Rayon> Rayons { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalImage> AnimalImages { get; set; }

    }
}

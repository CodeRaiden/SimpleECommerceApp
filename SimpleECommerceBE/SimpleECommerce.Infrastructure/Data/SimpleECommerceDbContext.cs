using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Domain.Entities;

namespace SimpleECommerce.Infrastructure.Data
{
    public class SimpleECommerceDbContext : DbContext
    {
        public SimpleECommerceDbContext(DbContextOptions<SimpleECommerceDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<CartItemEntity>()
                .HasKey(ci => new { ci.UserId, ci.ProductId });

            mb.Entity<CartItemEntity>()
                .HasOne(ci => ci.Product)
                .WithMany() // If Product has no CartItems navigation
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // or Restrict if you want to prevent deletes

            mb.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            mb.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }

    // ECommerce.Infrastructure/CartItemEntity.cs
    public class CartItemEntity
    {
        public string UserId { get; set; } = default!;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; } = default!; // Optional EF navigation
    }
}

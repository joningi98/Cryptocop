
using Cryptocop.Software.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Contexts
{
    public class CryptocopDbContext : DbContext
    {
        public CryptocopDbContext(DbContextOptions<CryptocopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasMany(u => u.shoppingCarts)
                .WithOne(s => s.user);
           
            // PaymentCard
            modelBuilder.Entity<PaymentCard>()
                .HasOne(p => p.user)
                .WithMany(u => u.paymentCards);

            // Address
            modelBuilder.Entity<Address>()
                .HasOne(a => a.user)
                .WithMany(u => u.addresses);

            // ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.user)
                .WithMany(u => u.shoppingCarts)
                .HasForeignKey(u => u.UserId);
                
            // ShoppingCartItem
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(s => s.shoppingCart)
                .WithMany(i => i.shoppingCartItems)
                .HasForeignKey(i => i.ShoppingCartId);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.user)
                .WithMany(u => u.orders)
                .HasForeignKey(u => u.userId);

            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.order)
                .WithMany()
                .HasForeignKey(o => o.OrderId);         
        }

        // Setup dbsets
        public DbSet<Address> Addresses { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

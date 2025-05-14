using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Data
{
   public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<CostumerOrder> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(i => i.Items)
                .HasForeignKey(o => o.OrderId);
        }
    }

}
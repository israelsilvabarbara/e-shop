using Basket.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Basket.API.Data
{
    public class BasketContext : DbContext
    {
        public DbSet<BasketSelection> Baskets { get; set; }
        
        public BasketContext(DbContextOptions<BasketContext> options, ILogger<BasketContext> logger)
            : base(options)
        {}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BasketSelection>().ToCollection("baskets");
        }
    }
}

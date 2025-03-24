using Inventory.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Inventory.API.Data
{
    public class InventoryContext : DbContext
    {
        public DbSet<InventoryItem> Inventorys { get; set; }
        public InventoryContext(DbContextOptions<InventoryContext> options, ILogger<InventoryContext> logger) 
        : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InventoryItem>().ToCollection("items");
        }
    }
}
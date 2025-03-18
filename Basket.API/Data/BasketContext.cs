using Basket.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Basket.API.Data
{
    public class BasketContext : DbContext
    {
        public DbSet<BasketSelection> Baskets { get; init; }

        public BasketContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BasketSelection>().ToCollection("baskets");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "27017"; // Default MongoDB port
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "BasketDB";
 
            // Construct the connection string
            var connectionString = string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPass)
                ? $"mongodb://{dbHost}:{dbPort}" // Without authentication
                : $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";
 
            optionsBuilder.UseMongoDB(connectionString, dbName);

        }
    }

}

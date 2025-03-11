using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data {
    
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        { }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType>  CatalogTypes { get; set; }
        public DbSet<CatalogItem>  CatalogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-many relationship between CatalogBrand and CatalogItem
            modelBuilder.Entity<CatalogItem>()
                .HasOne(ci => ci.CatalogBrand)
                .WithMany(cb => cb.CatalogItems)
                .HasForeignKey(ci => ci.CatalogBrandId);

            // One-to-many relationship between CatalogType and CatalogItem
            modelBuilder.Entity<CatalogItem>()
                .HasOne(ci => ci.CatalogType)
                .WithMany(ct => ct.CatalogItems)
                .HasForeignKey(ci => ci.CatalogTypeId);
        }
    }

}

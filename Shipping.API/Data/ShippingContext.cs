using Shipping.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Shipping.API.Data {
    
    public class ShippingContext : DbContext
    {
        public ShippingContext(DbContextOptions<CatalogContext> options) 
        : base(options)
        {}

        public DbSet<CatalogBrand> Adress { get; set; }

    }

}

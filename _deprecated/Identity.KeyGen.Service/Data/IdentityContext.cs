using Identity.KeyGen.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.KeyGen.Service.Data
{
    public class IdentityContext : DbContext
    {
        public DbSet<KeyVault> KeyVaults { get; set; }
        
        public IdentityContext(DbContextOptions<IdentityContext> options)
            :base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
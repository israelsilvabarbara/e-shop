using Identity.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class IdentityContext : IdentityDbContext<User>
    {
        DbSet<KeyVault> KeyVaults { get; set; }
        
        public IdentityContext(DbContextOptions<IdentityContext> options)
            :base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
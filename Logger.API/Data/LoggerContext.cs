using Logger.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Logger.API.Data {
    
    public class LoggerContext : DbContext
    {
        public LoggerContext(DbContextOptions<LoggerContext> options) 
        : base(options)
        {}

        public DbSet<LogMessage> Messages { get; set; }
        public DbSet<LogConsumer> Consumers { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LogConsumer>()
                .HasOne(lc => lc.LogMessage)
                .WithMany(l => l.Consumers)
                .HasForeignKey(lc => lc.LogMessageId);
        }
    }
}

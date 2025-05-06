using Identity.KeyGen.Service.Data;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge;
using Shared.EventBridge.Extensions;

namespace Identity.KeyGen.Service.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase()
                            .AddEventBus( consumerTypes: []);
            builder.Services.AddScoped<KeyUpdateExecutor>();

            return builder; 
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "26016";
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "identityDb";
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";


            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
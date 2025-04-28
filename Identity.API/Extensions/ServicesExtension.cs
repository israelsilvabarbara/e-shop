using Identity.API.Data;
using Identity.API.Models;
using Identity.API.EventConsumers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;

namespace Identity.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase()
                            .AddIdentity()
                            .AddEventBus( consumerTypes: [ typeof(IdentityKeyGeneratedEventConsumer) ] );
                    
            return builder;
        }
        
        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "28018";
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "identityDb";
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            
            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddAuthorization()
                            .AddAuthentication()
                            .AddBearerToken(IdentityConstants.BearerScheme);

            services.AddIdentityCore<User>()
                            .AddEntityFrameworkStores<IdentityContext>()
                            .AddApiEndpoints();
            return services;
        }
    }
}

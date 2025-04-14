using Identity.API.Data;
using Identity.API.Models;
using Inventory.API.EventBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                   .AddIdentity()
                   .AddEventBus( consumerTypes: [ typeof(IdentityKeyGeneratedEventConsumer) ] );
                    
            return builder;
        }
        
        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "28018";
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "identityDb";
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            
            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            Console.WriteLine("INFO: Using connection string: " + connectionString);


            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }

        private static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization()
                            .AddAuthentication()
                            .AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddIdentityCore<User>()
                            .AddEntityFrameworkStores<IdentityContext>()
                            .AddApiEndpoints();
            return builder;
        }
    }
}

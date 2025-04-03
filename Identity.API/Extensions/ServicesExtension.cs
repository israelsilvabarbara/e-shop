using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                   .AddIdentity();
            
            return builder;
        }


        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            string dbHost,dbPort,dbUser,dbPass,dbName;

            dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "27019";
            dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "identityDB";
        
            var connectionString = $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";

            Console.WriteLine("INFO: Using connection string: " + connectionString);
            Console.WriteLine("INFO: Using database name: " + dbName);
            
            // Add DbContext to the service collection
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseMongoDB(connectionString, dbName));


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

using FluentValidation;
using Inventory.API.Data;
using Inventory.API.DTOs;
using Inventory.API.EventBus;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;

namespace Inventory.API.Extensions
{
    public static class InventoryExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddSettings();

            builder.Services.AddDatabase()
                            .AddFluentValidation()
                            .AddEventBus( consumerTypes: [typeof(ProductCreatedEventConsumer),
                                                          typeof(ProductDeletedEventConsumer)  ]);
            return builder;
        }


        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            string dbHost,dbPort,dbUser,dbPass,dbName;

            // Retrieve environment variables for MongoDB configuration
            dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "29019";
            dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "inventoryDb";
            
            // Build the connection string
            var connectionString = $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";

            Console.WriteLine("INFO: Using connection string: " + connectionString);
            Console.WriteLine("INFO: Using database name: " + dbName);
            
            // Add DbContext to the service collection
            services.AddDbContext<InventoryContext>(options =>
                options.UseMongoDB(connectionString, dbName));


            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining <GetItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining <CreateItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining <UpdateItemRequestValidator>();
            return services;
        }


        


        private static WebApplicationBuilder AddSettings(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(  "inventorySettings.json", 
                                                optional: true, 
                                                reloadOnChange: true       );
            return builder;
        }

    }
}
using Basket.API.Data;
using Basket.API.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;

namespace Basket.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddDatabase()
                            .AddFluentValidation()
                            .AddEventBus( consumerTypes: [] );
            
            return builder; // Return the builder to support fluent chaining
        }


        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            string dbHost,dbPort,dbUser,dbPass,dbName;
            // Retrieve environment variables for MongoDB configuration
            dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "############################################";
            dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "basketDb";

            var connectionString = $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}/?authSource=admin";
            // Add DbContext to the service collection
            services.AddDbContext<BasketContext>(options =>
                options.UseMongoDB(connectionString, dbName));


            return services;
        }


        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<BasketItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateBasketItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateBasketItemRequestValidator>();

            return services;
        }

    }
}

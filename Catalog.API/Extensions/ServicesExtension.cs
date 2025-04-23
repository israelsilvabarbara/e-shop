using Catalog.API.Data;
using Catalog.API.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;

namespace Catalog.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase()
                            .AddFluentValidation()
                            .AddEventBus( consumerTypes: []);

            return builder; 
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort  = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "catalogDb";
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            
            services.AddValidatorsFromAssemblyContaining<CreateItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<FilterRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertBrandRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertTypeRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemListRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<PaginationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateItemRequestValidator>();
            return services;
        }
    }
}
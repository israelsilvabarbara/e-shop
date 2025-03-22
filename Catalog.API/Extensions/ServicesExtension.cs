using Catalog.API.Data;
using Catalog.API.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                    .AddFluentValidation();

            return builder; 
        }

        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUName = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS");
            var connectionString = $"Host={dbHost};Database={dbName};Username={dbUName};Password={dbPass};";

            builder.Services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }

        private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddValidatorsFromAssemblyContaining<CreateItemRequestValidator>();
            return builder;
        }
    }
}
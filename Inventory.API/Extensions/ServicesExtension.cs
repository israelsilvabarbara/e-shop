using FluentValidation;
using Inventory.API.Data;
using Inventory.API.DTOs;
using Inventory.API.EventBus;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;
using Shared.Keycloak.Extensions;

namespace Inventory.API.Extensions
{
    public static class InventoryExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddDatabase(configuration)
                            .AddFluentValidation()
                            .AddKeycloakAuthentication(configuration)
                            .AddEventBus(configuration,
                                        consumerTypes: [typeof(ProductCreatedEventConsumer),
                                                        typeof(ProductDeletedEventConsumer)  ]);
            return builder;
        }


        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var dbHost = configuration["database:host"]!;
                var dbPort = configuration["database:port"]!;
                var dbName = configuration["database:name"]!;
                var dbUser = configuration["database:user"]!;
                var dbPass = configuration["database:pass"]!;

                var connectionString = $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";

                services.AddDbContext<InventoryContext>(options =>
                    options.UseMongoDB(connectionString, dbName));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Did you forget to add AddDatabase to the configuration?");
            }
            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<GetItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateItemRequestValidator>();
            return services;
        }
    }
}
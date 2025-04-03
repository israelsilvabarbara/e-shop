using FluentValidation;
using Inventory.API.Data;
using Inventory.API.DTOs;
using Inventory.API.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Extensions
{
    public static class InventoryExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                   .AddFluentValidation()
                   .AddRabbitMq()
                   .AddSettings();

            return builder;            
        }


        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
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
            builder.Services.AddDbContext<InventoryContext>(options =>
                options.UseMongoDB(connectionString, dbName));


            return builder;
        }

        private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining <GetItemRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining <CreateItemRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining <UpdateItemRequestValidator>();
            return builder;
        }

        private static WebApplicationBuilder AddRabbitMq(this WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(config =>
            {
                AddConsumers(config);
                config.SetKebabCaseEndpointNameFormatter();
                config.UsingRabbitMq((context, configurator)=>
                {
                    string host,port,user,pass;

                    // Retrieve environment variables for MongoDB configuration
                    host = Environment.GetEnvironmentVariable("EVENT_HOST") ?? "localhost";
                    port = Environment.GetEnvironmentVariable("EVENT_PORT") ?? "5672";
                    user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "admin";
                    pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "password";

                    configurator.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
            
            return builder;
        }

        private static void AddConsumers(IBusRegistrationConfigurator config)
        {
            config.AddConsumer<ProductCreatedEventConsumer>();
            config.AddConsumer<ProductDeletedEventConsumer>();
            // Add more consumers as needed
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
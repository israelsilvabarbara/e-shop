using Identity.API.Data;
using Identity.API.Models;
using Inventory.API.EventBus;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Identity.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                   .AddIdentity()
                   .AddRabbitMq();
                    
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

        private static void AddConsumers(IBusRegistrationConfigurator config)
        {
            config.AddConsumer<IdentityKeyGeneratedEventConsumer>();
            // Add more consumers as needed
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
                    user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "guest";
                    pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "guest";

                    configurator.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });
                });
            });

            return builder;
        }
    }
}

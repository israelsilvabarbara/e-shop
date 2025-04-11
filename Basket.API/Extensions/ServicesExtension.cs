using Basket.API.Data;
using Basket.API.DTOs;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {

            builder.AddDatabase()
                   .AddFluentValidation();
                  // .AddRabbitMq();
            
            return builder; // Return the builder to support fluent chaining
        }


        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            string dbHost,dbPort,dbUser,dbPass,dbName;
            // Retrieve environment variables for MongoDB configuration
            dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "############################################";
            dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "basketDb";

            var connectionString = $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}/?authSource=admin";

            Console.WriteLine("********************INFO: Using connection string: " + connectionString);
            Console.WriteLine("********************INFO: Using database name: " + dbName);
            
            // Add DbContext to the service collection
            builder.Services.AddDbContext<BasketContext>(options =>
                options.UseMongoDB(connectionString, dbName));


            return builder;
        }


        private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<BasketItemRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketItemRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketItemRequestValidator>();

            return builder;
        }

        private static WebApplicationBuilder AddRabbitMq(this WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                config.UsingRabbitMq((context, configurator)=>
                {
                    string host,port,user,pass;
                    if ( builder.Environment.IsDevelopment() )
                    {
                        host = "localhost";
                        port = "5672";
                        user = "guest";
                        pass = "guest";
                    }else
                    {
                        // Retrieve environment variables for MongoDB configuration
                        host = Environment.GetEnvironmentVariable("EVENT_HOST") ?? "";
                        port = Environment.GetEnvironmentVariable("EVENT_PORT") ?? "";
                        user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "";
                        pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "";
                    }

                    if( string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || 
                        string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass)    )
                    {
                        throw new ArgumentException($"RabbitMQ configuration is missing or invalid.\nEVENT_HOST: {host}, EVENT_PORT: {port}, EVENT_USER: {user}, EVENT_PASS: {pass} environment variables are required.");
                    } 

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
    }
}

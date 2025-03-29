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
                   .AddFluentValidation()
                   .AddRabbitMq();
            
            return builder; // Return the builder to support fluent chaining
        }


        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            string dbHost,dbPort,dbUser,dbPass,dbName;

            if ( builder.Environment.IsDevelopment() )
            {
                dbHost = "localhost";
                dbPort = "27017";
                dbUser = "admin";
                dbPass = "secure-password";
                dbName = "basketDB";
            }else
            {
                // Retrieve environment variables for MongoDB configuration
                dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
                dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "";
                dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
                dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "";
                dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
            }

            if ( string.IsNullOrWhiteSpace(dbHost) || 
                 string.IsNullOrWhiteSpace(dbPort) || 
                 string.IsNullOrWhiteSpace(dbName)   )
            {
                throw new ArgumentException($"Database configuration is missing or invalid for Basket API.\nDB_HOST: {dbHost}, DB_PORT: {dbPort}, DB_NAME: {dbName} environment variables are required.");
            }

            var connectionString = string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPass)
                ? $"mongodb://{dbHost}:{dbPort}" // Without authentication
                : $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";

            Console.WriteLine("INFO: Using connection string: " + connectionString);
            Console.WriteLine("INFO: Using database name: " + dbName);
            
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

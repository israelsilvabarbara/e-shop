using Catalog.API.Data;
using Catalog.API.DTOs;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
                   .AddFluentValidation()
                   .AddRabbitMq();

            return builder; 
        }

        private static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "26016";
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "catalogDb";
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
            var connectionString = $"Host={dbHost};Database={dbName};Username={dbUName};Password={dbPass};";

            builder.Services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }

        private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddValidatorsFromAssemblyContaining<CreateItemRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<FilterRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<InsertBrandRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<InsertTypeRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ItemListRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<PaginationRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateItemRequestValidator>();
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
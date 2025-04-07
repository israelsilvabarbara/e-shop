using Identity.KeyGen.Service.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.KeyGen.Service.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.AddDatabase()
        
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
            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

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

                    configurator.ConfigureEndpoints(context);
                });
            });
            
            return builder;
        }
    }
}
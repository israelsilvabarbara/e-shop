using MassTransit;
using Microsoft.Extensions.DependencyInjection;


namespace Shared.EventBridge.Extensions
{
    public static class EventBusExtension
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services,IEnumerable<Type>? consumerTypes = null)
        {
            services.AddScoped<EventBus>();
            services.AddMassTransit(config =>
            {
                AddConsumers(config,consumerTypes);
                config.SetKebabCaseEndpointNameFormatter();
                config.UsingRabbitMq((context, configurator) =>
                {
                    var host = Environment.GetEnvironmentVariable("EVENT_HOST") ?? "israel";
                    var port = Environment.GetEnvironmentVariable("EVENT_PORT") ?? "9999";
                    var user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "guest";
                    var pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "guest";

                    configurator.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });

                    Console.WriteLine("#########################################");
                    Console.WriteLine("#########################################"); 
                    Console.WriteLine($"INFO: Using RabbitMQ host: {host}:{port}");
                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }


        private static void AddConsumers(IBusRegistrationConfigurator config,IEnumerable<Type>? consumerTypes)
        {
            if ( consumerTypes == null) return;
            foreach (var consumer in consumerTypes)
            {
                config.AddConsumer(consumer);
            }
        }
    }

}

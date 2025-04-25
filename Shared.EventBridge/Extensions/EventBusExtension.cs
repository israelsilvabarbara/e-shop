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
                    var host = Environment.GetEnvironmentVariable("EVENT_HOST") ?? "";
                    var port = Environment.GetEnvironmentVariable("EVENT_PORT") ?? "";
                    var user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "";
                    var pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "";

                    configurator.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });
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

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Shared.EventBridge.Extensions
{
    public static class EventBusExtension
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services,
                                                          IConfiguration configuration,
                                                          IEnumerable<Type>? consumerTypes = null)
        {
            try
            {
                services.AddScoped<EventBus>();
                services.AddMassTransit(config =>
                {
                    AddConsumers(config, consumerTypes);
                    config.SetKebabCaseEndpointNameFormatter();
                    config.UsingRabbitMq((context, configurator) =>
                    {
                        var host = configuration["eventbus:host"]!;
                        var port = configuration["eventbus:port"]!;
                        var user = configuration["eventbus:user"]!;
                        var pass = configuration["eventbus:pass"]!;

                        configurator.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                        {
                            h.Username(user);
                            h.Password(pass);
                        });
                        configurator.ConfigureEndpoints(context);
                    });
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding event bus: " + ex.Message);
                Console.WriteLine("Did you forget to call AddEventBus to builder.Configuration  ?");
            }
            return services;
        }


        private static void AddConsumers(IBusRegistrationConfigurator config, IEnumerable<Type>? consumerTypes)
        {
            if (consumerTypes == null) return;
            foreach (var consumer in consumerTypes)
            {
                config.AddConsumer(consumer);
            }
        }
    }

}

using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class EventBusExtension
{
    public static WebApplicationBuilder AddEventBus(this WebApplicationBuilder builder,IEnumerable<Type> consumerTypes)
    {
        builder.Services.AddScoped<EventBridge>();
        builder.Services.AddScoped<Logger>();
        builder.Services.AddMassTransit(config =>
        {
            AddConsumers(config,consumerTypes);
            config.SetKebabCaseEndpointNameFormatter();
            config.UsingRabbitMq((context, configurator) =>
            {
                var host = Environment.GetEnvironmentVariable("EVENT_HOST") ?? "localhost";
                var port = Environment.GetEnvironmentVariable("EVENT_PORT") ?? "5672";
                var user = Environment.GetEnvironmentVariable("EVENT_USER") ?? "guest";
                var pass = Environment.GetEnvironmentVariable("EVENT_PASS") ?? "guest";

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


    private static void AddConsumers(IBusRegistrationConfigurator config,IEnumerable<Type> consumerTypes)
    {
        foreach (var consumer in consumerTypes)
        {
            config.AddConsumer(consumer);
        }
    }
}

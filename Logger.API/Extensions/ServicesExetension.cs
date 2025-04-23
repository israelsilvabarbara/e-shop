

using FluentValidation;
using Logger.API.Data;
using Logger.API.DTOs;
using Logger.API.EventBus;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Extensions;

public static class ServicesExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDatabase()
                        .AddFluentValidation()
                        .AddEventBus( [ typeof(LogEventConsumer),
                                        typeof(LogConsumedEventConsumer)]);
        return builder;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ServiceSelectorRequestValidator>();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {

        var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var dbPort  = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ?? "loggerDb";
        var dbUName = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
        var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ?? "secure-password";
        var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

        services.AddDbContext<LoggerContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
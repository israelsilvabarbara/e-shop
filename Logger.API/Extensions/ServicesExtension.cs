

using FluentValidation;
using Logger.API.Data;
using Logger.API.DTOs;
using Logger.API.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.EventBridge.Extensions;
using Shared.Keycloak.Extensions;


namespace Logger.API.Extensions
{

    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddDatabase(configuration)
                            .AddSwagger()
                            .AddFluentValidation()
                            .AddKeycloakAuthentication(configuration)
                            .AddEventBus(configuration,
                                        consumerTypes: [ typeof(LogEventConsumer),
                                                     typeof(LogConsumedEventConsumer)]);
            return builder;
        }

         private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "Logger API",
                            Version = "v1"
                        });
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "Enter 'Bearer {token}'"
                        });
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                        });
                 });

            return services;

        }


        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ServiceSelectorRequestValidator>();
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbHost = configuration["database:host"];
            var dbPort = configuration["database:port"];
            var dbName = configuration["database:name"];
            var dbUName = configuration["database:user"];
            var dbPass = configuration["database:pass"];

            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            services.AddDbContext<LoggerContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
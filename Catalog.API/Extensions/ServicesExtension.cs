using System.Reflection;
using Catalog.API.Data;
using Catalog.API.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.EventBridge.Extensions;
using Shared.Keycloak.Extensions;

namespace Catalog.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            builder.Services.AddSwagger()
                            .AddDatabase(config)
                            .AddFluentValidation()
                            .AddEventBus( consumerTypes: [])
                            .AddKeycloakAuthentication(config);

            return builder; 
        }



        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "Catalog API",
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
        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            
            var dbHost = configuration["database:host"];
            var dbPort = configuration["database:port"];
            var dbName = configuration["database:name"];
            var dbUName = configuration["database:user"];
            var dbPass = configuration["database:pass"];

            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            
            services.AddValidatorsFromAssemblyContaining<CreateItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertBrandRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertTypeRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<FilterQueryRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<PaginationQueryRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateItemRequestValidator>();
            return services;
        }


    }
}
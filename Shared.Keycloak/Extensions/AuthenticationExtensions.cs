using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Keycloak.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakUrl = configuration["keycloak:url"];
            var keycloakPort = configuration["keycloak:port"];
            var realm = configuration["keycloak:realm"];
            var audience = configuration["keycloak:audience"];
            var authority = $"{keycloakUrl}:{keycloakPort}/realms/{realm}";
            var isRequiredHttpsMetadata = configuration["keycloak:requireHttpsMetadata"] == "true";
            
            services.AddAuthorization()
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = authority;
                        options.Audience = audience;
                        options.RequireHttpsMetadata = isRequiredHttpsMetadata;
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                                context.Token = token;
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                // Custom validation logic can go here if needed
                                return Task.CompletedTask;
                            }
                        };
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authority,
                            ValidateAudience = true,
                            ValidAudience = audience,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true
                        };
                    });

            return services;
        }
    }
}

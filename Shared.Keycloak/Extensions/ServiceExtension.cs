using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Keycloak.Services;
using Microsoft.AspNetCore.Authentication;

namespace Shared.Keycloak.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
                 // Get Keycloak variables from configuration
            var keycloakInternalUrl = configuration["keycloak:url"]; // Inside Docker
            var keycloakInternalPort = configuration["keycloak:port"];
            var keycloakApiUrl = configuration["keycloak:apiUrl"]; // External (Frontend, other services)
            var keycloakApiPort = configuration["keycloak:apiPort"];
            var realm = configuration["keycloak:realm"];
            var audience = configuration["keycloak:audience"];
            var isRequiredHttpsMetadata = configuration["keycloak:requireHttpsMetadata"] == "true";

            // Set external authority for issuer validation
            var authority = $"{keycloakApiUrl}:{keycloakApiPort}/realms/{realm}";
            // Set internal Keycloak URL for fetching public keys
            var keycloakCertsUrl = $"{keycloakInternalUrl}:{keycloakInternalPort}/realms/{realm}/protocol/openid-connect/certs";

          
            services.AddAuthorization( options =>
                    {
                        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                        options.AddPolicy("User", policy => policy.RequireRole("User", "Admin"));    
                    })
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
                                var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                                context.Token = token;
                                return Task.CompletedTask;
                            }
                        };
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authority,

                            ValidateAudience = true,
                            ValidAudiences = [audience],
                            
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                            {
                                // Fetch public keys dynamically from Keycloak
                                var client = new HttpClient();
                                var json = client.GetStringAsync(keycloakCertsUrl).Result;

                                var keys = new JsonWebKeySet(json).Keys;
                                return keys.Where(k => k.Kid == kid);
                            },
                            ValidateLifetime = true,
                        };
                    });

            services.AddScoped<IClaimsTransformation, KeycloakClaimsTransformer>();

            return services;
        }
    }
}

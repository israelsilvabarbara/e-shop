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

            Console.WriteLine("##############################################################");
            Console.WriteLine($"DEBUG: Keycloak Internal URL: {keycloakInternalUrl}");
            Console.WriteLine($"DEBUG: Keycloak Internal Port: {keycloakInternalPort}");
            Console.WriteLine($"DEBUG: Keycloak API URL: {keycloakApiUrl}");
            Console.WriteLine($"DEBUG: Keycloak API Port: {keycloakApiPort}");
            Console.WriteLine($"DEBUG: Keycloak Realm: {realm}");
            Console.WriteLine($"DEBUG: Keycloak Audience: {audience}");
            Console.WriteLine($"DEBUG: Keycloak Authority (Issuer Validation): {authority}");
            Console.WriteLine($"DEBUG: Keycloak Certs URL: {keycloakCertsUrl}");
            Console.WriteLine($"DEBUG: Require HTTPS Metadata: {isRequiredHttpsMetadata}");
            Console.WriteLine("##############################################################");

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

                                Console.WriteLine("DEBUG: Found {0} keys", keys.Count);
                                var keyList = keys.Where(k => k.Kid == kid).ToList(); 

                                if (keyList.Count == 0)
                                {
                                    Console.WriteLine("DEBUG: No matching key found");
                                }else
                                {
                                    Console.WriteLine("###########################################################");
                                    Console.WriteLine("DEBUG: Found {0} matching keys", keyList.Count);
                                    foreach (var key in keyList)
                                    {
                                        Console.WriteLine("DEBUG: Key: {0}", key.KeyId);
                                    }
                                    Console.WriteLine("###########################################################");
                                }
                                
                                return keys.Where(k => k.Kid == kid);
                            },
                            ValidateLifetime = true
                        };
                    });

            return services;
        }
    }
}

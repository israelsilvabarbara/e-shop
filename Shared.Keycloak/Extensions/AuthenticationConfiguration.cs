using Microsoft.Extensions.Configuration;

namespace Shared.Keycloak.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static IConfigurationBuilder AddKeycloakAuthentication(this IConfigurationBuilder builder)
        {
            var keycloakUrl = Environment.GetEnvironmentVariable("KEYCLOAK_URL");
            var keycloakPort = Environment.GetEnvironmentVariable("KEYCLOAK_PORT");
            var realm = Environment.GetEnvironmentVariable("KEYCLOAK_REALM");
            var audience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE");
            var RequireHttpsMetadata = Environment.GetEnvironmentVariable("KEYCLOAK_REQUIRE_HTTPS_METADATA");

            if (string.IsNullOrEmpty(keycloakUrl))
            {
                throw new Exception("KEYCLOAK_URL environment variable is not set");
            }

            if (string.IsNullOrEmpty(keycloakPort))
            {
                throw new Exception("KEYCLOAK_PORT environment variable is not set");
            }

            if (string.IsNullOrEmpty(realm))
            {
                throw new Exception("KEYCLOAK_REALM environment variable is not set");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new Exception("KEYCLOAK_AUDIENCE environment variable is not set");
            }

            if (string.IsNullOrEmpty(RequireHttpsMetadata))
            {
                throw new Exception("KEYCLOAK_REQUIRE_HTTPS_METADATA environment variable is not set"); ;
            }

            var dictionary = new Dictionary<string, string?>()
            {
                { "keycloak:url", keycloakUrl },
                { "keycloak:port", keycloakPort },
                { "keycloak:realm", realm },
                { "keycloak:audience", audience },
                { "keycloak:requireHttpsMetadata", RequireHttpsMetadata }
            };

            return builder.AddInMemoryCollection(dictionary);
        }
    }
}
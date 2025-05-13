using Microsoft.Extensions.Configuration;

namespace Shared.Keycloak.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static IConfigurationBuilder AddKeycloakAuthentication(this IConfigurationBuilder builder)
        {
            var errors = new List<string>();
            var keycloakUrl = Environment.GetEnvironmentVariable("KEYCLOAK_URL");
            var keycloakPort = Environment.GetEnvironmentVariable("KEYCLOAK_PORT");
            var keycloakAPIUrl = Environment.GetEnvironmentVariable("KEYCLOAK_API_URL");
            var keycloakAPIPort = Environment.GetEnvironmentVariable("KEYCLOAK_API_PORT");
            var realm = Environment.GetEnvironmentVariable("KEYCLOAK_REALM");
            var audience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE");
            var RequireHttpsMetadata = Environment.GetEnvironmentVariable("KEYCLOAK_REQUIRE_HTTPS_METADATA");

            if (string.IsNullOrEmpty(keycloakUrl))
            {
                errors.Add("KEYCLOAK_URL environment variable is not set");
            }

            if (string.IsNullOrEmpty(keycloakPort))
            {
                errors.Add("KEYCLOAK_PORT environment variable is not set");
            }

            if (string.IsNullOrEmpty(keycloakAPIUrl))
            {
                errors.Add("KEYCLOAK_API_URL environment variable is not set");
            }

            if (string.IsNullOrEmpty(keycloakAPIPort))
            {
                errors.Add("KEYCLOAK_API_PORT environment variable is not set");
            }

            if (string.IsNullOrEmpty(realm))
            {
                errors.Add("KEYCLOAK_REALM environment variable is not set");
            }

            if (string.IsNullOrEmpty(audience))
            {
                errors.Add("KEYCLOAK_AUDIENCE environment variable is not set");
            }

            if (string.IsNullOrEmpty(RequireHttpsMetadata))
            {
                errors.Add("KEYCLOAK_REQUIRE_HTTPS_METADATA environment variable is not set"); ;
            }


            if (errors.Any())
            {
                throw new Exception(string.Join("\n", errors));
            }

            var dictionary = new Dictionary<string, string?>()
            {
                { "keycloak:url", keycloakUrl },
                { "keycloak:port", keycloakPort },
                { "keycloak:apiUrl", keycloakAPIUrl },
                { "keycloak:apiPort", keycloakAPIPort },
                { "keycloak:realm", realm },
                { "keycloak:audience", audience },
                { "keycloak:requireHttpsMetadata", RequireHttpsMetadata }
            };

            return builder.AddInMemoryCollection(dictionary);
        }
    }
}
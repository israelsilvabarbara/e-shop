
using Shared.EventBridge.Extensions;
using Shared.Keycloak.Extensions;

namespace Logger.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            var useMockConfig = builder.Configuration.GetValue<bool>("UseMockConfig");

            if (useMockConfig)
            {
                builder.Configuration.AddMockConfig();
            }else
            {
                builder.Configuration.AddDatabase()
                                    .AddSwagger()
                                    .AddEventBus()
                                    .AddKeycloakAuthentication();
            }

            return builder;
        }


        private static IConfigurationBuilder AddDatabase(this IConfigurationBuilder builder)
        {
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort  = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUName = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS");


            if(string.IsNullOrEmpty(dbHost))
            {
                throw new Exception("DB_HOST environment variable is not set");
            }

            if(string.IsNullOrEmpty(dbPort))
            {
                throw new Exception("DB_PORT environment variable is not set");
            }

            if(string.IsNullOrEmpty(dbName))
            {
                throw new Exception("DB_NAME environment variable is not set");
            }

            if(string.IsNullOrEmpty(dbUName))
            {
                throw new Exception("DB_USER environment variable is not set");
            }

            if(string.IsNullOrEmpty(dbPass))
            {
                throw new Exception("DB_PASS environment variable is not set");
            }

            var dictionary = new Dictionary<string,string?>
            {
                { "database:host", dbHost },
                { "database:port", dbPort },
                { "database:name", dbName },
                { "database:user", dbUName },
                { "database:pass", dbPass }
            };
            return builder.AddInMemoryCollection(dictionary);
        }


        private static IConfigurationBuilder AddSwagger(this IConfigurationBuilder builder)
        {
            var swagger = Environment.GetEnvironmentVariable("SWAGGER_ENABLED");
            
            if (string.IsNullOrEmpty(swagger))
            {
                throw new Exception("SWAGGER_ENABLED environment variable is not set");
            }

            swagger = swagger.ToLower();

            var dictionary = new Dictionary<string, string?>
            {
                { "swagger:enabled", swagger }
            };
            return builder.AddInMemoryCollection(dictionary);
        }

        private static IConfigurationBuilder AddMockConfig(this IConfigurationBuilder builder)
        {
            var dictionary = new Dictionary<string, string?>
            {
                { "database:host", "logger-db" },
                { "database:port", "5432" },
                { "database:name", "loggerDb" },
                { "database:user", "admin" },
                { "database:pass", "secure-password" },
                { "swagger:enabled", "true" },
                { "eventbus:host", "rabbitmq" },
                { "eventbus:port", "5672" },
                { "eventbus:user", "admin" },
                { "eventbus:pass", "password" },
                { "keycloak:url",  "http://localhost"}, // running from outside docker
                { "keycloak:port", "9010" },
                { "keycloak:apiUrl", "http://localhost" },
                { "keycloak:apiPort", "9010" },
                { "keycloak:realm", "ecommerce-realm" },
                { "keycloak:audience", "mock-frontend" },
                { "keycloak:requireHttpsMetadata", "false" }
            };

            return builder.AddInMemoryCollection(dictionary);
        }
    }
}
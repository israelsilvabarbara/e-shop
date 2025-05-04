using Shared.Middleware.Authentication;


namespace Identity.API.Extensions
{
    public static class ConfigurationExtension
    {

        public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddSwagger()
                                 .AddDatabase()
                                 .AddSecrets();
            return builder;
        }


        private static IConfigurationBuilder AddSwagger(this IConfigurationBuilder builder)
        {
            var swagger = Environment.GetEnvironmentVariable("SWAGGER_ENABLED");
            
            if (string.IsNullOrEmpty(swagger))
            {
                throw new Exception("SWAGGER_ENABLED environment variable is not set");
            }

            swagger = swagger.ToLower();

            var swaggerDictionary = new Dictionary<string, string?>
            {
                { "swagger:enabled", swagger }
            };
            return builder.AddInMemoryCollection(swaggerDictionary);
        }

        private static IConfigurationBuilder AddDatabase(this IConfigurationBuilder builder)
        {
            var dbHost  = Environment.GetEnvironmentVariable("DB_HOST") ;
            var dbPort  = Environment.GetEnvironmentVariable("DB_PORT") ;
            var dbName  = Environment.GetEnvironmentVariable("DB_NAME") ;
            var dbUName = Environment.GetEnvironmentVariable("DB_USER") ;
            var dbPass  = Environment.GetEnvironmentVariable("DB_PASS") ;

            if ( string.IsNullOrEmpty(dbHost) )
            {
                throw new Exception("DB_HOST environment variable is not set");
            }

            if ( string.IsNullOrEmpty(dbPort))
            {
                throw new Exception("DB_PORT environment variable is not set");
            }

            if ( string.IsNullOrEmpty(dbName))
            {
                throw new Exception("DB_NAME environment variable is not set");
            }

            if ( string.IsNullOrEmpty(dbUName))
            {
                throw new Exception("DB_USER environment variable is not set");
            }

            if ( string.IsNullOrEmpty(dbPass))
            {
                throw new Exception("DB_PASS environment variable is not set");
            }

            var databaseDictionary = new Dictionary<string, string?>
            {
                { "database:host", dbHost },
                { "database:port", dbPort },
                { "database:name", dbName },
                { "database:user", dbUName },
                { "database:pass", dbPass }
            };
            return builder.AddInMemoryCollection(databaseDictionary);
        }
    }
}
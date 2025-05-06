using Shared.Middleware.Authentication;


namespace Identity.API.Extensions
{
    public static class ConfigurationExtension
    {

        public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJWT()
                                 .AddSwagger()
                                 .AddDatabase()
                                 .AddSecrets();
            return builder;
        }


        private static IConfigurationBuilder AddJWT(this IConfigurationBuilder builder)
        {
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (string.IsNullOrEmpty(jwtIssuer))
            {
                throw new Exception("JWT_ISSUER environment variable is not set");
            }

            if (string.IsNullOrEmpty(jwtAudience))
            {
                throw new Exception("JWT_AUDIENCE environment variable is not set");    
            }

            var dictionary = new Dictionary<string, string?>
            {
                { "jwt:issuer", jwtIssuer },
                { "jwt:audience", jwtAudience }
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

            var dictionary = new Dictionary<string, string?>
            {
                { "database:host", dbHost },
                { "database:port", dbPort },
                { "database:name", dbName },
                { "database:user", dbUName },
                { "database:pass", dbPass }
            };
            return builder.AddInMemoryCollection(dictionary);
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Shared.Middleware.Authentication
{
    public static class SecretsExtensions
    {
        public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder)
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("JWT_SECRET environment variable is not set");
            }


            var secretsDictionary = new Dictionary<string, string?>
            {
                { "Jwt:Secret", jwtSecret }
            };

            return builder.AddInMemoryCollection(secretsDictionary);
        }
    }
}

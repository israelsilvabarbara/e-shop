
using Microsoft.Extensions.Configuration;

namespace Shared.Middleware.Authentication
{
    public static class SecretsExtensions
    {
        public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder)
        {
            var jwtSecret = File.ReadAllText("/run/secrets/jwt_secret");

            var secretsDictionary = new Dictionary<string, string?>
            {
                { "Jwt:Secret", jwtSecret }
            };

            return builder.AddInMemoryCollection(secretsDictionary);
        }
    }
}

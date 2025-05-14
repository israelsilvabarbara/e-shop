

using Microsoft.Extensions.Configuration;

namespace Shared.EventBridge.Extensions
{
    public static class ConfigurationExtension
    {
        public static IConfigurationBuilder AddEventBus(this IConfigurationBuilder builder)
        {
            var errors = new List<string>();
            var host = Environment.GetEnvironmentVariable("EVENT_HOST");
            var port = Environment.GetEnvironmentVariable("EVENT_PORT");
            var user = Environment.GetEnvironmentVariable("EVENT_USER");
            var pass = Environment.GetEnvironmentVariable("EVENT_PASS");


            if (string.IsNullOrEmpty(host))
            {
                errors.Add("EVENT_HOST environment variable is not set");
            }

            if(string.IsNullOrEmpty(port))
            {
                errors.Add("EVENT_PORT environment variable is not set");
            }

            if(string.IsNullOrEmpty(user)) 
            {
                errors.Add("EVENT_USER environment variable is not set");
            }

            if(string.IsNullOrEmpty(pass))
            {
                errors.Add("EVENT_PASS environment variable is not set");
            }

            if(errors.Count > 0)
            {
                throw new Exception(string.Join("\n", errors));
            }


            var dictionary = new Dictionary<string, string?>()
            {
                { "eventbus:host", host },
                { "eventbus:port", port },
                { "eventbus:user", user },
                { "eventbus:pass", pass }
            };
            
            return builder.AddInMemoryCollection(dictionary);
        }
    }
}
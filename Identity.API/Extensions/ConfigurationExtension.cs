using Shared.Middleware.Authentication;


namespace Identity.API.Extensions
{
    public static class ConfigurationExtension
    {

        public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddSecrets();
            return builder;
        }
    }
}
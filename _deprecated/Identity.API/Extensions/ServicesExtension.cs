using System.Text;
using Identity.API.Data;
using Identity.API.EventConsumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.EventBridge.Extensions;

namespace Identity.API.Extensions
{
    public static class ServicesExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Identity API",
                    Version = "v1"
                });
            });

            builder.Services.AddDatabase(builder.Configuration)
                            .AddIdentity(builder.Configuration)
                            .AddEventBus(consumerTypes: [typeof(IdentityKeyGeneratedEventConsumer)]);

            return builder;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            var dbHost = configuration["database:host"];
            var dbPort = configuration["database:port"];
            var dbName = configuration["database:name"];
            var dbUName = configuration["database:user"];
            var dbPass = configuration["database:pass"];

            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUName};Password={dbPass};";

            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
        /* 
                private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
                {
                    services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<IdentityContext>()
                            .AddDefaultTokenProviders()
                            .AddApiEndpoints();


                    services.AddAuthorization()
                            .AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                            .AddJwtBearer(options =>
                            {
                                var jwtSecret = configuration["jwt:secret"];
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                                };
                            });

                    //services.AddIdentityCore<IdentityUser>()

                    return services;
                } */


        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Identity with EF Core stores, token providers, and API endpoints.
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();

            // Configure authentication to use JWT Bearer.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var jwtSecret = configuration["jwt:secret"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["jwt:issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["jwt:audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
                };
            });

            services.AddAuthorization();

            return services;
        }

    }
}

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read the token from the Authorization header (Bearer token)
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Token is missing.");
                return;
            }

            try
            {
                // Retrieve the symmetric key from configuration.
                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                var secret = configuration["Jwt:Secret"];

                if (string.IsNullOrEmpty(secret))
                {
                    throw new InvalidOperationException("JWT secret is not configured.");
                }

                var key = Encoding.UTF8.GetBytes(secret);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,    // Adjust if you want to validate Issuer
                    ValidateAudience = false,  // Adjust if you want to validate Audience
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Attempt to validate the token.
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // If token is valid, proceed to the next middleware.
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401;  // Unauthorized
                await context.Response.WriteAsync($"Token validation failed: {ex.Message}");
            }
        }
    }
}

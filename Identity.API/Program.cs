using Identity.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Shared.Middleware.Authentication;

var builder = WebApplication.CreateBuilder(args);

//builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

var swaggerEnabled = Environment.GetEnvironmentVariable("SWAGGER_ENABLED")?.ToLower() == "true";
swaggerEnabled = true;
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthenticationMiddleware();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapIdentityApi<IdentityUser>();



app.Run();

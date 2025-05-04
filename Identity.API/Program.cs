using Identity.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Shared.Middleware.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

var swaggerEnabled = builder.Configuration["swagger:enabled"] == "true";

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

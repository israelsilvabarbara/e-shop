using Identity.API.Extensions;
using Identity.API.Models;
using Shared.Middleware.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

app.UseAuthenticationMiddleware();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapIdentityApi<User>();



app.Run();

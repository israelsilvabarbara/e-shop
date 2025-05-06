using Identity.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Shared.Middleware.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

var swaggerEnabled = builder.Configuration["swagger:enabled"]!.Equals("true", StringComparison.CurrentCultureIgnoreCase);

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();


app.Run();

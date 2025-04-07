

using Identity.API.Extensions;
using Identity.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapIdentityApi<User>();



app.Run();

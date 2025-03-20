using Basket.API.Endpoints;
using Basket.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.MapBasketEndpoints();
app.UseHttpsRedirection();

app.Run();

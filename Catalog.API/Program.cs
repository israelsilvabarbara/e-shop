using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.MapCatalogEndpoints();
app.UseHttpsRedirection();

app.Run();

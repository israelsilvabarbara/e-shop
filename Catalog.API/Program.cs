using Catalog.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var dbHost  = Environment.GetEnvironmentVariable("DB_HOST");
var dbName  = Environment.GetEnvironmentVariable("DB_NAME");
var dbUName = Environment.GetEnvironmentVariable("DB_USER");
var dbPass  = Environment.GetEnvironmentVariable("DB_PASS");
var connectionString = $"Host={dbHost};Database={dbName};Username={dbUName};Password={dbPass};";

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddLogging(config => {
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}
app.MapCatalogEndpoints();
app.UseHttpsRedirection();
app.Run();

using Catalog.API.Data;
using Microsoft.EntityFrameworkCore;



public static class MigrationExtensions {
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
        
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CatalogContext>();
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}


using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        
        app.MapGet("/Catalog/", async (CatalogContext context) =>
        {
            var items = await context.CatalogItems.ToListAsync();
            return Results.Ok(items);
        });
        
        app.MapGet("/catalog/{id}", async (int id, CatalogContext context) =>
        {
            var item = await context.CatalogItems.FindAsync(id);
            return item != null ? Results.Ok(item) : Results.NotFound();
        });

        app.MapPost("/catalog", async (CatalogItem item, CatalogContext context) =>
        {
            context.CatalogItems.Add(item);
            await context.SaveChangesAsync();
            return Results.Created($"/api/catalog/{item.Id}", item);
        });

        app.MapPut("/catalog/{id}", async (int id, CatalogItem updatedItem, CatalogContext context) =>
        {
            var item = await context.CatalogItems.FindAsync(id);
            if (item == null) return Results.NotFound();

            item.Name = updatedItem.Name;
            item.Price = updatedItem.Price;
            item.Description = updatedItem.Description;
            // Update other properties as needed...

            await context.SaveChangesAsync();
            return Results.NoContent();
        });


    }
    
}
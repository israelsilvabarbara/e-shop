

using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        
        app.MapGet("/catalog", async (PaginationRequest pagination, FilterRequest filter, CatalogContext context) =>
        {
            var filteredCatalog = context.CatalogItems.AsQueryable();

            // Apply filters
            if (filter.MinPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.CatalogType))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogType.Type == filter.CatalogType);

            if (!string.IsNullOrWhiteSpace(filter.CatalogBrand))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogBrand.Brand == filter.CatalogBrand);

            // Apply pagination
            var paginatedCatalog = filteredCatalog
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var count = await filteredCatalog.CountAsync();

            var paginationResponse = new PaginationResponse<CatalogItem>(paginatedCatalog, count);

            return Results.Ok(paginationResponse);
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
using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        
        app.MapGet("/catalog", async (PaginationRequest pagination, FilterRequest filter, CatalogContext context) =>
        {
                var filteredCatalog = context.CatalogItems
                    .Include(p => p.CatalogBrand) // Eagerly load CatalogBrand
                    .Include(p => p.CatalogType)  // Eagerly load CatalogType
                    .AsQueryable();

            // Apply filters
            if (filter.MinPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.Type))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogType.Type == filter.Type);

            if (!string.IsNullOrWhiteSpace(filter.Brand))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogBrand.Brand == filter.Brand);

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

        app.MapPost("/catalog", async (CreateItemRequest request, CatalogContext context) =>
        {

          
            var catalogItem = new CatalogItem
            {
                Name = request.Name,
                Description = request.Description,
                PictureFileName = request.PictureFileName,
                PictureUri = $"https://your-storage-url/{request.PictureFileName}", // Optional mapping for PictureUri
                Price = request.Price,
                CatalogBrandId = request.CatalogBrandId,
                CatalogTypeId = request.CatalogTypeId,
                AvailableStock = request.AvailableStock,
                RestockThreshold = request.RestockThreshold,
                MaxStockThreshold = request.MaxStockThreshold
            };

            context.CatalogItems.Add(catalogItem);
            await context.SaveChangesAsync();
            return Results.Created($"/api/catalog/{catalogItem.Id}", catalogItem);
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


        app.MapGet("/Catalog/types", async (CatalogContext context) => 
        { 
            var types = await context.CatalogTypes.ToListAsync();
            return Results.Ok(types);
        });


        app.MapPost("/catalog/types", async (string typeName, CatalogContext context) =>
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return Results.BadRequest("Type name cannot be empty.");
            }

            var newCatalogType = new CatalogType
            {
                Type = typeName
            };

            context.CatalogTypes.Add(newCatalogType);
            await context.SaveChangesAsync();

            return Results.Created($"/catalog/types/{newCatalogType.Id}", newCatalogType);
        });


        app.MapDelete("/catalog/types/{id}", async (int id, CatalogContext context) =>
        {
            var type = await context.CatalogTypes.FindAsync(id);
            if (type == null) return Results.NotFound();

            context.CatalogTypes.Remove(type);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapGet("/catalog/brands", async (CatalogContext context) => 
        {
            var brands = await context.CatalogBrands.ToListAsync();
            return Results.Ok(brands);
        });

        app.MapPost("/catalog/brands", async (string brand, CatalogContext context) =>
        {
            if(string.IsNullOrWhiteSpace(brand))
            {
                return Results.BadRequest("Brand name cannot be empty.");
            }

            var newBrand = new CatalogBrand
            {
                Brand = brand
            };

            context.CatalogBrands.Add(newBrand);
            await context.SaveChangesAsync();

            return Results.Created($"/catalog/brands/{newBrand.Id}", newBrand);

        });

        app.MapDelete("/catalog/brands/{id}", async (int id, CatalogContext context) =>
        {
            var brand = await context.CatalogBrands.FindAsync(id);
            if (brand == null) return Results.NotFound();    

            context.CatalogBrands.Remove(brand);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

    }
    
}
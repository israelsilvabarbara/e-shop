using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        
        app.MapGet("/catalog", listItems);
        app.MapGet("/catalog/{id}", getItem );
        app.MapPost("/catalog", insertItem);
        app.MapPut("/catalog/{id}", updateItem);
        app.MapDelete("/catalog/{id}", deleteItem);
        
        app.MapGet("/Catalog/type", listTypes);
        app.MapGet("/catalog/type/{id}", getItem);
        app.MapPost("/catalog/type", insertType);
        app.MapDelete("/catalog/type/{id}", deleteType);

        app.MapGet("/catalog/brand", listBrands);
        app.MapGet("/catalog/brand/{id}", getItem);
        app.MapPost("/catalog/brand", insertBrand);
        app.MapDelete("/catalog/brands/{id}", deleteBrand ); 
        
        

        static async Task<IResult> listItems(
            [FromBody] ItemListRequest request, 
            [FromServices] IValidator<ItemListRequest> validator, 
            [FromServices] CatalogContext context)
        {

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var filteredCatalog = context.CatalogItems
                .Include(p => p.CatalogBrand) // Eagerly load CatalogBrand
                .Include(p => p.CatalogType)  // Eagerly load CatalogType
                .AsQueryable();

            // Apply filters

            var filter = request.Filter;
            if (filter.MinPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                filteredCatalog = filteredCatalog.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.Type))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogType.Type == filter.Type);

            if (!string.IsNullOrWhiteSpace(filter.Brand))
                filteredCatalog = filteredCatalog.Where(p => p.CatalogBrand.Brand == filter.Brand);

            // Apply pagination
            var pagination = request.Pagination;
            var paginatedCatalog = filteredCatalog
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var count = await filteredCatalog.CountAsync();

            var paginationResponse = new PaginationResponse<CatalogItem>(paginatedCatalog, count);

            return Results.Ok(paginationResponse);
        }

        
        static async Task<IResult> getItem(int id, CatalogContext context)
        {
            var item = await context.CatalogItems.FindAsync(id);
            return item != null ? Results.Ok(item) : Results.NotFound();
        }


        static async Task<IResult> insertItem(
            [FromBody] CreateItemRequest request,
            [FromServices] IValidator<CreateItemRequest> validator, 
            [FromServices] CatalogContext context)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var catalogItem = new CatalogItem
            {
                Name = request.Name,
                Description = request.Description,
                PictureFileName = request.PictureFileName,
                PictureUri = $"https://your-storage-url/{request.PictureFileName}", // Optional mapping for PictureUri
                Price = request.Price,
                CatalogBrandId = request.CatalogBrandId,
                CatalogTypeId = request.CatalogTypeId
            };

            context.CatalogItems.Add(catalogItem);
            await context.SaveChangesAsync();
            return Results.Created($"/api/catalog/{catalogItem.Id}", catalogItem);
        };

        
        static async Task<IResult> updateItem(
            [FromBody] UpdateItemRequest request,
            [FromServices] IValidator<UpdateItemRequest> validator,
            [FromServices] CatalogContext context)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var item = await context.CatalogItems.FirstOrDefaultAsync(i => i.Id == request.Id);

            if (item == null) return Results.NotFound();

            if(request.Name != null) item.Name = request.Name;
            if(request.Description != null) item.Description = request.Description;
            if(request.PictureFileName != null) item.PictureFileName = request.PictureFileName;
            if(request.PictureUrl != null) item.PictureUri = request.PictureUrl;
            if(request.Price != null) item.Price = (decimal)request.Price;
            if(request.Brand != null) 
            {
                var brand = await context.CatalogBrands.FirstOrDefaultAsync(b => b.Brand == request.Brand);
                if(brand == null)
                {
                    Results.BadRequest($"Brand:{request.Brand} not found");
                }
                item.CatalogBrandId = brand!.Id;
            }

            if(request.Type != null) {
                var type = await context.CatalogTypes.FirstOrDefaultAsync(t => t.Type == request.Type);
                if(type == null)
                {
                    Results.BadRequest($"Type:{request.Type} not found");
                }
                item.CatalogTypeId = type!.Id;
            }

            await context.SaveChangesAsync();
            return Results.NoContent();
        }


        static async Task<IResult> deleteItem(
            [FromRoute] int id, 
            [FromServices] CatalogContext context)
        {
            var item = await context.CatalogItems.FindAsync(id);
            if (item == null) return Results.NotFound();

            context.CatalogItems.Remove(item);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        static async Task<IResult> listTypes(
            [FromServices] CatalogContext context) 
        { 
            var types = await context.CatalogTypes.ToListAsync();
            return Results.Ok(types);
        }

        static async Task<IResult> getType(
            [FromRoute] int id, 
            [FromServices] CatalogContext context)
        {
            var type = await context.CatalogTypes.FindAsync(id);
            return type != null ? Results.Ok(type) : Results.NotFound();
        }

        static async Task<IResult> insertType(
            [FromRoute] string typeName, 
            [FromServices] CatalogContext context )
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
        }


        static async Task<IResult> deleteType(
            [FromRoute] int id, 
            [FromServices] CatalogContext context )
        {
            var type = await context.CatalogTypes.FindAsync(id);
            if (type == null) return Results.NotFound();

            context.CatalogTypes.Remove(type);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
        
        static async Task<IResult> listBrands(
            [FromServices] CatalogContext context ) 
        {
            var brands = await context.CatalogBrands.ToListAsync();
            return Results.Ok(brands);
        }

        static async Task<IResult> getBrand(int id, CatalogContext context)
        {
            var brand = await context.CatalogBrands.FindAsync(id);
            return brand != null ? Results.Ok(brand) : Results.NotFound();
        }

        static async Task<IResult> insertBrand(
            [FromRoute] string brand, 
            [FromServices] CatalogContext context )
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

        }

        static async Task<IResult> deleteBrand (
            [FromRoute] int id, 
            [FromServices] CatalogContext context ) 
        {
            var brand = await context.CatalogBrands.FindAsync(id);
            if (brand == null) return Results.NotFound();    

            context.CatalogBrands.Remove(brand);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
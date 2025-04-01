using System;
using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Models;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) 
    {
        app.MapGet("/catalog/ping", () => "pong");
        app.MapGet("/catalog/list", ListItems);
        app.MapGet("/catalog/search/{id:guid}", SearchItem);
        app.MapPost("/catalog/insert", InsertItem);
        app.MapPut("/catalog/update", UpdateItem);
        app.MapDelete("/catalog/delete/{id:guid}", DeleteItem); 
       
        app.MapGet("/catalog/type/list", ListTypes);
        app.MapGet("/catalog/type/search/{id:guid}", SearchType);
        app.MapPost("/catalog/type/insert", InsertType);
        app.MapDelete("/catalog/type/delete/{id:guid}", DeleteType);
        
        app.MapGet("/catalog/brand/list", ListBrands);
        app.MapGet("/catalog/brand/search/{id:guid}", SearchBrand);
        app.MapPost("/catalog/brand/insert", InsertBrand);
        app.MapDelete("/catalog/brand/delete/{id:guid}", DeleteBrand ); 

        app.MapFallback(NotFoundEndpoint); 
    }
        

        
        

    static async Task<IResult> ListItems(
        HttpContext httpContext, 
        [FromServices] IValidator<ItemListRequest> validator, 
        [FromServices] CatalogContext context)
    {


        Console.WriteLine("DEBUG: listItems");
        var query = httpContext.Request.Query;

        Console.WriteLine("DEBUG query: " + query);

        var request = ItemListRequest.FromQuery(query);

        if (request == null)
        {
            return Results.BadRequest("Invalid request");
        }

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

        var paginationResponse = new PaginationResponse<CatalogItem>( count, request.Pagination, request.Filter, paginatedCatalog );

        return Results.Ok(paginationResponse);
    }

    
    static async Task<IResult> SearchItem(
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context)
    {
        var item = await context.CatalogItems.FirstOrDefaultAsync(i => i.Id == id);
        return item != null ? Results.Ok(item) : Results.NotFound();
    }


    static async Task<IResult> InsertItem(
        [FromBody] CreateItemRequest request,
        [FromServices] IValidator<CreateItemRequest> validator, 
        [FromServices] CatalogContext context,
        [FromServices] IPublishEndpoint publishEndpoint)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }

        var catalogItem = new CatalogItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            PictureFileName = request.PictureFileName,
            PictureUri = $"https://your-storage-url/{request.PictureFileName}",
            Price = request.Price,
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId
        };

        context.CatalogItems.Add(catalogItem);
        await context.SaveChangesAsync();


        var productCreatedEvent = new ProductCreatedEvent(
                ProductId: catalogItem.Id,
                ProductName: catalogItem.Name,
                EventDate: DateTime.UtcNow
        );

        await publishEndpoint.Publish( productCreatedEvent );
        return Results.Created($"/api/catalog/{catalogItem.Id}", new InsertedItemResponse( Id: catalogItem.Id,
            Name: catalogItem.Name,
            Description: catalogItem.Description,
            PictureFileName: catalogItem.PictureFileName,
            PictureUri: catalogItem.PictureUri,
            Price: catalogItem.Price,
            CatalogBrandId: catalogItem.CatalogBrandId,
            CatalogTypeId: catalogItem.CatalogTypeId));
    }

    
    static async Task<IResult> UpdateItem(
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


    static async Task<IResult> DeleteItem(
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context)
    {
        var item = await context.CatalogItems.FindAsync(id);
        if (item == null) return Results.NotFound();

        context.CatalogItems.Remove(item);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    static async Task<IResult> ListTypes(
        [FromServices] CatalogContext context) 
    { 
        var types = await context.CatalogTypes.ToListAsync();

        List<ItemResponse> itemsResponse = types.Select(t => new ItemResponse(Id:t.Id, Name:t.Type)).ToList();


        return Results.Ok(new ListTypesResponse(Count: itemsResponse.Count, Types: itemsResponse));
    }

    static async Task<IResult> SearchType(
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context)
    {
        var type = await context.CatalogTypes.FindAsync(id);
        return type != null ? Results.Ok(new ItemResponse(Id:type.Id, Name:type.Type)) : 
                              Results.NotFound();
    }

    static async Task<IResult> InsertType(
        [FromBody] InsertTypeRequest request, 
        [FromServices] IValidator<InsertTypeRequest> validator,
        [FromServices] CatalogContext context )
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }


    // Filter out duplicates by checking the database
        var existingTypes = await context.CatalogTypes
                                        .Where(ct => request.Types.Contains(ct.Type))
                                        .Select(ct => ct.Type)
                                        .ToListAsync();

        var filteredTypes = request.Types
                                .Except(existingTypes) // Remove duplicates
                                .Select(typeName => new CatalogType
                                {
                                    Id = Guid.NewGuid(),
                                    Type = typeName
                                })
                                .ToList();

        
        context.CatalogTypes.AddRange(filteredTypes);
        await context.SaveChangesAsync();

        List<ItemResponse> itemResponse = filteredTypes.Select(t => new ItemResponse(Id:t.Id, Name:t.Type)).ToList();

        return Results.Created($"/catalog/types/insert", new ListTypesResponse( Count: itemResponse.Count, Types:itemResponse ));
    }


    static async Task<IResult> DeleteType(
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context )
    {
        var type = await context.CatalogTypes.FirstOrDefaultAsync( t => t.Id== id);
        if (type == null) return Results.NotFound();

        context.CatalogTypes.Remove(type);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    
    static async Task<IResult> ListBrands(
        [FromServices] CatalogContext context ) 
    {
        var brands = await context.CatalogBrands.ToListAsync();

        List<ItemResponse> brandsResponse = brands.Select(b => new ItemResponse(Id:b.Id, Name:b.Brand)).ToList();
        
        return Results.Ok(new ListBrandsResponse( Count: brandsResponse.Count, Brands: brandsResponse));
    }

    static async Task<IResult> SearchBrand(
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context)
    {
        var brand = await context.CatalogBrands.FindAsync(id);
        return brand != null ? Results.Ok(new ItemResponse(Id:brand.Id, Name:brand.Brand)) : 
                               Results.NotFound();
    }

    static async Task<IResult> InsertBrand(
        [FromBody] InsertBrandRequest request,
        [FromServices] IValidator<InsertBrandRequest> validator, 
        [FromServices] CatalogContext context )
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }

        var existingBrands = await context.CatalogBrands
                                            .Where(cb => request.Brands.Contains(cb.Brand))
                                            .Select(cb => cb.Brand)
                                            .ToListAsync();
        
        var filteredBrands = request.Brands.Except(existingBrands)
                                            .Select(brandName => new CatalogBrand
                                            {
                                                Id = Guid.NewGuid(),
                                                Brand = brandName
                                            })
                                            .ToList();

        context.CatalogBrands.AddRange(filteredBrands);
        await context.SaveChangesAsync();

        List<ItemResponse> itemsResponse = filteredBrands.Select(b => new ItemResponse(Id:b.Id, Name:b.Brand)).ToList();

        return Results.Created($"/catalog/brands/insert", new ListBrandsResponse( Count: itemsResponse.Count, Brands: itemsResponse));
    }

    static async Task<IResult> DeleteBrand (
        [FromRoute] Guid id, 
        [FromServices] CatalogContext context ) 
    {
        var brand = await context.CatalogBrands.FirstOrDefaultAsync( b => b.Id == id);
        if (brand == null) return Results.NotFound();    

        context.CatalogBrands.Remove(brand);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    
    static IResult NotFoundEndpoint(HttpContext context)
    {
        context.Response.StatusCode = 404;
        return Results.NotFound(new { error = "The requested endpoint was not found." });
    }
}
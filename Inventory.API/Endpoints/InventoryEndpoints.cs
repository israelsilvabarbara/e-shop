using FluentValidation;
using Inventory.API.Data;
using Inventory.API.DTOs;
using Inventory.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory/list", ListItems);
        app.MapGet("/inventory/search/{productId:guid}", GetItem);
        app.MapGet("/inventory/filter", GetItems);
        app.MapPost("/inventory/insert", InsertItem);
        app.MapPut("/inventory/update", UpdateItem);
        app.MapPut("/inventory/restock", RestockItem);
    }
    
    static async Task<IResult> ListItems([FromServices] InventoryContext context)
    {
        var items = await context.Inventorys.ToListAsync();

        if( items == null )
        {
            return Results.NotFound();
        }

        var summary = items
            .Select(i => new InventoryDetailResponse
            (
                ProductId: i.ProductId,
                Stock: i.Stock
            ))
            .ToList();

        return Results.Ok(new ListInventoryResponse(Count: summary.Count, Items: summary));
    }

    static async Task<IResult> GetItem(
        [FromRoute] Guid productId,
        [FromServices] InventoryContext context )
    {

        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProductId == productId);

        if (item == null)
        {
            var errorResponse = new
            {
                Message = "Product not found",
                ProductId = productId
            };
            
            return Results.NotFound(errorResponse);
        }

        return Results.Ok(new InventoryDetailResponse(ProductId: item.ProductId, Stock: item.Stock));
    } 

    static async Task<IResult> GetItems(
        HttpContext httpContext,
        [FromServices] IValidator<GetItemsRequest> validator,
        [FromServices] InventoryContext context )
    {
        var query = httpContext.Request.Query;

        var request = GetItemsRequest.FromQuery(query);
        if (request == null)
        {
            return Results.BadRequest("The query contains empty or malformed values.");
        }
    
        var validatorREsult = validator.Validate(request);
        if (!validatorREsult.IsValid)
        {
            return Results.BadRequest(validatorREsult.ToDictionary());
        }

        var items = await context.Inventorys
            .Where(i => request.ProductIds.Contains(i.ProductId))
            .ToListAsync();

        if (items == null )
        {
            return Results.NotFound("Product not found");
        }

        var itemsResponse = items.Select(i => new InventoryDetailResponse(ProductId: i.ProductId, Stock: i.Stock));

        return Results.Ok(new ListInventoryResponse(Count: itemsResponse.Count(), Items: itemsResponse)); 
        
    }


    static async Task<IResult> InsertItem(
        [FromBody] CreateItemRequest request,
        [FromServices] IValidator<CreateItemRequest> validator,
        [FromServices] InventoryContext context )
    {
        var validatorResultt = validator.Validate(request);

        if (!validatorResultt.IsValid)
        {
            return Results.BadRequest(validatorResultt.ToDictionary());
        }

        if (await context.Inventorys.AnyAsync(i => i.ProductId == request.ProductId ))
        {
            return Results.BadRequest("Product already exists");
        }

        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Stock = request.Stock,
            StockThreshold = request.StockThreshold
        };

        context.Add(item);

        await context.SaveChangesAsync();

        return Results.Created($"/inventory/{item.ProductId}", item);
    }


    static async Task<IResult> UpdateItem(
        [FromBody] UpdateItemRequest request,
        [FromServices] IValidator<UpdateItemRequest> validator,
        [FromServices] InventoryContext context )
    {   
        var validatorResult = validator.Validate(request);

        if (!validatorResult.IsValid)
        {
            return Results.BadRequest(validatorResult.ToDictionary());
        }    

        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProductId == request.ProductId);

        if (item == null)
        {
            return Results.NotFound("Product not found");
        }

        if( request.ProductName != null )   item.ProductName = request.ProductName;
        if( request.Stock != null )         item.Stock = (int)request.Stock;
        if( request.StockThreshold != null ) item.StockThreshold = (int)request.StockThreshold;
       
        await context.SaveChangesAsync();

        return Results.Ok(item);
    }

    static async Task<IResult> RestockItem(
        [FromBody] RestockItemRequest request,
        [FromServices] IValidator<RestockItemRequest> validator,
        [FromServices] InventoryContext context )
    {
        var validatorResult = validator.Validate(request);

        if (!validatorResult.IsValid)
        {
            return Results.BadRequest(validatorResult.ToDictionary());
        }

        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProductId == request.ProductId);

        if (item == null)
        {
            return Results.NotFound("Product not found");
        }

        item.Stock += (int)request.Quantity;

        await context.SaveChangesAsync();

        var restockResponse = new RestockResponse(
            ProductId: item.ProductId,
            ProductName: item.ProductName,
            Stock: item.Stock
        );
        return Results.Ok(restockResponse);
    }


 
}
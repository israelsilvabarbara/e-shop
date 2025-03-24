using FluentValidation;
using Inventory.API.Data;
using Inventory.API.DTOs;
using Inventory.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory", listItems);
        app.MapGet("/inventory/{productId}", getItem);
        app.MapGet("/inventory/filter", getItems);
        app.MapPost("/inventory", insertItem);
        app.MapPut("/inventory/{id}", updateItem);
        app.MapDelete("/inventory/{id}", deleteItem);
    }
    
    static async Task<IResult> listItems([FromServices] InventoryContext context)
    {
        var items = await context.Inventorys.ToListAsync();

        if( items == null  || items.Count == 0 )
        {
            return Results.NotFound();
        }

        var summary = items
            .Select(i => new InventorySummaryResponse
            (
                ProductId: i.ProdutctId,
                Stock: i.Stock
            ))
            .ToList();

        return Results.Ok(summary);
    }

    static async Task<IResult> getItem(
        [FromRoute] int productId,
        [FromServices] InventoryContext context )
    {
        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProdutctId == productId);

        if (item == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(item);
    } 

    static async Task<IResult> getItems(
        [FromBody] GetItemsRequest request,
        [FromServices] IValidator<GetItemsRequest> validator,
        [FromServices] InventoryContext context )
    {
        var validatorREsult = validator.Validate(request);

        if (!validatorREsult.IsValid)
        {
            return Results.BadRequest(validatorREsult.ToDictionary());
        }

        var items = await context.Inventorys
            .Where(i => request.ProductIds.Contains(i.ProdutctId))
            .ToListAsync();

        if (items == null || items.Count == 0)
        {
            return Results.NotFound();
        }

        return Results.Ok(items);
        
    }


    static async Task<IResult> insertItem(
        [FromBody] CreateItemRequest request,
        [FromServices] IValidator<CreateItemRequest> validator,
        [FromServices] InventoryContext context )
    {
        var validatorResultt = validator.Validate(request);

        if (!validatorResultt.IsValid)
        {
            return Results.BadRequest(validatorResultt.ToDictionary());
        }

        if (await context.Inventorys.AnyAsync(i => i.ProdutctId == request.ProductId))
        {
            return Results.BadRequest("Product already exists");
        }

        var item = new InventoryItem
        {
            ProdutctId = request.ProductId,
            ProductName = request.ProductName,
            Stock = request.Stock,
            StockTresholdMin = request.StockTresholdMin,
            StockTresholdMax = request.StockTresholdMax,
        };

        context.Add(item);

        await context.SaveChangesAsync();

        return Results.Created($"/inventory/{item.ProdutctId}", item);
    }


    static async Task<IResult> updateItem(
        [FromBody] UpdateItemRequest request,
        [FromServices] IValidator<UpdateItemRequest> validator,
        [FromServices] InventoryContext context )
    {   
        var validatorResult = validator.Validate(request);

        if (!validatorResult.IsValid)
        {
            return Results.BadRequest(validatorResult.ToDictionary());
        }    

        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProdutctId == request.ProductId);

        if (item == null)
        {
            return Results.NotFound();
        }

        if( request.ProductName != null )   item.ProductName = request.ProductName;
        if( request.Stock != null )         item.Stock = (int)request.Stock;
        if( request.StockTresholdMin != null ) item.StockTresholdMin = (int)request.StockTresholdMin;
        if( request.StockTresholdMax != null ) item.StockTresholdMax = (int)request.StockTresholdMax;

        await context.SaveChangesAsync();

        return Results.Ok(item);
    }


    static async Task<IResult> deleteItem(
        [FromRoute] int productId,
        [FromServices] InventoryContext context )
    {
        var item = await context.Inventorys.FirstOrDefaultAsync(i => i.ProdutctId == productId);

        if (item == null)
        {
            return Results.NotFound();
        }

        context.Remove(item);

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
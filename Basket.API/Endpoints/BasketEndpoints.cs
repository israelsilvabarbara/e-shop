using System.Security.Claims;
using Basket.API.Data;
using Basket.API.DTOs;
using Basket.API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Basket.API.Endpoints
{
    public static class BasketEndpoints
    {
        public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
        {
            var protectedGroup = app.MapGroup("/basket").RequireAuthorization();

            protectedGroup.MapGet("/", GetBasket);
            
            protectedGroup.MapPost("/item", AddItemToBasket);
            protectedGroup.MapPut("/items/{itemId}", UpdateItem); 
            protectedGroup.MapPut("/items/{itemId}/increment", IncrementItemQuantity);
            protectedGroup.MapPut("/items/{itemId}/decrement", DecrementItemQuantity);
            
            protectedGroup.MapDelete("/", DeleteBasket);
            protectedGroup.MapDelete("/item/{itemId}", DeleteBasketItem);
        }


        public static async Task<IResult> GetBasket(
            ClaimsPrincipal user, 
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if(string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var items = basket.Items;

            return Results.Ok(items);
        }


        public static async Task<IResult> AddItemToBasket( 
            ClaimsPrincipal user,
            [FromBody] CreateBasketItemRequest request, 
            [FromServices] IValidator<CreateBasketItemRequest> validator, 
            [FromServices] BasketContext context)
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket =  await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                basket  = new BasketSelection
                {
                    BuyerId = buyerId,
                    Items = new List<Models.BasketItem>(),
                };

                await context.Baskets.AddAsync(basket);
            }

            var BasketItem = basket.Items.FirstOrDefault( i => i.ProductId == request.ProductId);

            if (BasketItem == null)
            {
                basket.Items.Add(new Models.BasketItem
                {
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                    PictureUrl = request.PictureUrl
                });
            }
            else
            {
                BasketItem.Quantity += request.Quantity;
            }

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> UpdateItem(
            ClaimsPrincipal user,
            [FromBody] UpdateBasketItemRequest request,
            [FromServices] IValidator<UpdateBasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var validationResult = validator.Validate(request);

            if( !validationResult.IsValid )
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (BasketItem == null)
            {
                return Results.NotFound();
            }

            BasketItem.Quantity = request.Quantity;

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> IncrementItemQuantity(
            ClaimsPrincipal user,
            [FromBody] BasketItemRequest request,
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (basketItem == null)
            {
                return Results.NotFound();
            }

            basketItem.Quantity++;

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> DecrementItemQuantity( 
            ClaimsPrincipal user,
            [FromBody] BasketItemRequest request, 
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var validationResult = validator.Validate(request);

            if( !validationResult.IsValid )
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (basketItem == null)
            {
                return Results.NotFound();
            }

            if (basketItem.Quantity > 1)
            {
                basketItem.Quantity--;
            }

            await context.SaveChangesAsync();

            return Results.Ok();

        }


        private static async Task<IResult> DeleteBasket( 
            ClaimsPrincipal user,
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }
            
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            basket.Items.Clear();

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> DeleteBasketItem(
            ClaimsPrincipal user,
            [FromRoute] Guid productId,
            [FromServices] BasketContext context )
        {
            var buyerId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            if( productId == Guid.Empty )
            {
                return Results.BadRequest("ProductId is required.");
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if(basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId );

            if (BasketItem == null)
            {
                return Results.NotFound();
            }

            basket.Items.Remove(BasketItem);

            await context.SaveChangesAsync();

            return Results.Ok();
        }        
    }
}
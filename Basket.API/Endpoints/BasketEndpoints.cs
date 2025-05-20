using System.Security.Claims;
using Basket.API.Data;
using Basket.API.DTOs;
using Basket.API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Enums;
using Shared.Events;
using Shared.Keycloak.Tools;


namespace Basket.API.Endpoints
{
    public static class BasketEndpoints
    {
        public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
        {
            var protectedGroup = app.MapGroup("/basket").RequireAuthorization();

            protectedGroup.MapGet("/", GetBasket);

            protectedGroup.MapPost("/item", AddItemToBasket);
            protectedGroup.MapPut("/items", UpdateItem);
            protectedGroup.MapPut("/items/{itemId}/increment", IncrementItemQuantity);
            protectedGroup.MapPut("/items/{itemId}/decrement", DecrementItemQuantity);

            protectedGroup.MapDelete("/", DeleteBasket);
            protectedGroup.MapDelete("/item/{itemId}", DeleteBasketItem);

            protectedGroup.MapPost("/checkout", Checkout);
        }


        public static async Task<IResult> GetBasket(
            ClaimsPrincipal user,
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

            if (string.IsNullOrEmpty(buyerId))
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
            var buyerId = ClaimsHelper.GetUserId(user);

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
                basket = new BasketSelection
                {
                    BuyerId = buyerId,
                    Items = new List<Models.BasketItem>(),
                };

                await context.Baskets.AddAsync(basket);
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ItemId == request.ItemId);

            if (BasketItem == null)
            {
                basket.Items.Add(new Models.BasketItem
                {
                    ItemId = request.ItemId,
                    ItemName = request.ItemName,
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
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

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

            var BasketItem = basket.Items.FirstOrDefault(i => i.ItemId == request.ItemId);

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
            [AsParameters] BasketItemRequest request,
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

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

            var basketItem = basket.Items.FirstOrDefault(i => i.ItemId == request.ItemId);

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
            [AsParameters] BasketItemRequest request,
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

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

            var basketItem = basket.Items.FirstOrDefault(i => i.ItemId == request.ItemId);

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
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

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
            [FromRoute] Guid ItemId,
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            if (ItemId == Guid.Empty)
            {
                return Results.BadRequest("ItemId is required.");
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ItemId == ItemId);

            if (BasketItem == null)
            {
                return Results.NotFound();
            }

            basket.Items.Remove(BasketItem);

            await context.SaveChangesAsync();

            return Results.Ok();
        }


        private static async Task<IResult> Checkout(
            ClaimsPrincipal user,
            [FromServices] EventBus eventBus,
            [FromServices] BasketContext context)
        {
            var buyerId = ClaimsHelper.GetUserId(user);

            if (string.IsNullOrEmpty(buyerId))
            {
                return Results.Unauthorized();
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            //create the event , send and test and remove items from basket.
            var items = new List<ItemDetails>();
            foreach (var i in basket.Items)
            {
                items.Add(new ItemDetails
                (
                    Id: i.ItemId,
                    Name: i.ItemName,
                    Quantity: i.Quantity,
                    UnitPrice: i.UnitPrice
                ));
            }

            if(items.Count == 0 )
            {
                return Results.BadRequest("Basket is empty");
            }



            try
            {
                var checkoutEvent = new BasketCheckoutEvent
                (
                    Id: Guid.NewGuid(),
                    BasketId: Guid.Parse(basket.Id),
                    BuyerId: Guid.Parse(buyerId),
                    Items: items,
                    EventDate: DateTime.UtcNow
                );

                await eventBus.SendAsync(checkoutEvent,
                                Services.Basket,
                                LogEventType.Info,
                                LogStatus.Success,
                                $"Basket {basket.Id} checked out.");
   
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex);
            }

            basket.Items.Clear();
            await context.SaveChangesAsync();
            
            return Results.Ok();
        }

             
    }
}
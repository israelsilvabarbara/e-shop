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


            protectedGroup.MapGet("/list",ListBaskets);
            protectedGroup.MapGet("/search/{buyerId}", ListBasketItems);
            protectedGroup.MapPost("/item", InsertBasketItem);
            protectedGroup.MapPut("/item", UpdateBasket);
            protectedGroup.MapPut("/item/increment", IncrementBasketItemAmout);
            protectedGroup.MapPut("/item/decrement", DecrementBasketItemAmout);
            
            protectedGroup.MapDelete("/item/{buyerId}", DeleteBasket);
            protectedGroup.MapDelete("/item", DeleteBasketItem);
        }


        public static async Task<IResult> ListBaskets( [FromServices] BasketContext context )
        {
            var baskets = await context.Baskets.ToListAsync();


            Console.WriteLine("DEBUG: Baskets: " + baskets);
            //Console.WriteLine("INFO: Baskets: " + JsonSerializer.Serialize(baskets));
            return Results.Ok(baskets);
        }


        public static async Task<IResult> ListBasketItems(
            [FromRoute] string buyerId, 
            [FromServices] BasketContext context )
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyerId);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var items = basket.Items;

            return Results.Ok(items);
        }


        public static async Task<IResult> InsertBasketItem( 
            [FromBody] CreateBasketItemRequest request, 
            [FromServices] IValidator<CreateBasketItemRequest> validator, 
            [FromServices] BasketContext context)
        {

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket =  await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == request.BuyerId);

            if (basket == null)
            {
                basket  = new BasketSelection
                {
                    BuyerId = request.BuyerId,
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

        private static async Task<IResult> UpdateBasket(
            [FromBody] UpdateBasketItemRequest request,
            [FromServices] IValidator<UpdateBasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var validationResult = validator.Validate(request);

            if( !validationResult.IsValid )
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == request.BuyerId);

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

        private static async Task<IResult> IncrementBasketItemAmout(
            [FromBody] BasketItemRequest request,
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == request.BuyerId);

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

        private static async Task<IResult> DecrementBasketItemAmout( 
            [FromBody] BasketItemRequest request, 
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var validationResult = validator.Validate(request);

            if( !validationResult.IsValid )
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == request.BuyerId);

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
            string buyer,
            [FromServices] BasketContext context )
        {
            if (string.IsNullOrEmpty(buyer))
            {
                return Results.BadRequest();
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            basket.Items.Clear();

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> DeleteBasketItem(
            [FromBody] BasketItemRequest request,
            [FromServices] IValidator<BasketItemRequest> validator,
            [FromServices] BasketContext context )
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == request.BuyerId);

            if(basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId );

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
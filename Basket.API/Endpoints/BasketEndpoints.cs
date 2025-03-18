using Basket.API.Data;
using Microsoft.EntityFrameworkCore;


namespace Basket.API.Endpoints
{
    public static class BasketEndpoints
    {
        public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", listBasketItems)
                .WithName("GetBasket")
                .WithOpenApi();
            app.MapPost("/basket", insertBasketItem);
            app.MapPut("/basket", updateBasket);
            app.MapPut("/basket/add/{productId}", addBasketItemAmout);
            app.MapPut("/basket/sub/{productId}", subBasketItemAmout);
            app.MapDelete("/basket", deleteBasket);
            app.MapDelete("/basket/{productId}", deleteBasketItem);
        }



        public static async Task<IResult> listBasketItems(string buyer, BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var items = basket.Items;

            return Results.Ok(items);
        }


        public static async Task<IResult> insertBasketItem(string buyer , BasketItemRequest item, BasketContext context)
        {
            var basket =  await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault( i => i.ProductId == item.ProductId);

            if (BasketItem == null)
            {
                basket.Items.Add(new Models.BasketItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    PictureUrl = item.PictureUrl
                });
            }
            else
            {
                BasketItem.Quantity += item.Quantity;
            }

            await context.SaveChangesAsync();

            return Results.Ok();
        }
        private static async Task<IResult> updateBasket(string buyer, BasketItemRequest item,BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (BasketItem == null)
            {
                return Results.NotFound();
            }

            BasketItem.Quantity = item.Quantity;

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> addBasketItemAmout(string buyer,int productId,BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);

            if (basketItem == null)
            {
                return Results.NotFound();
            }

            basketItem.Quantity++;

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> subBasketItemAmout( string buyer,int productId, BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);

            if (basketItem == null)
            {
                return Results.NotFound();
            }

            if (basketItem.Quantity > 1)
            {
                basketItem.Quantity--;
            }
            else
            {
                // COULD BE IGNORED DEPENDING ON BUSINESS LOGIC
                basket.Items.Remove(basketItem);
            }

            await context.SaveChangesAsync();

            return Results.Ok();

        }


        private static async Task<IResult> deleteBasket( string buyer,BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if (basket == null)
            {
                return Results.NotFound();
            }

            basket.Items.Clear();

            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> deleteBasketItem(string buyer,int productId,BasketContext context)
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.BuyerId == buyer);

            if(basket == null)
            {
                return Results.NotFound();
            }

            var BasketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);

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
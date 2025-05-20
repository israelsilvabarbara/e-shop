
using System.Security.Claims;
using FluentValidation;
using MassTransit.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Models;
using Shared.Keycloak.Tools;

namespace Order.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var publicGroup = app.MapGroup("/order");
            var protectedGroup = app.MapGroup("/order").RequireAuthorization();
            var adminGroup = app.MapGroup("/order").RequireAuthorization("AdminOnly");

            publicGroup.MapGet("/health", () => "ok");

            protectedGroup.MapGet("", GetOrders);
            protectedGroup.MapGet("{id:guid}", GetOrderById);
            protectedGroup.MapPost("", InsertOrder);
            protectedGroup.MapPut("{id:guid}", UpdateOrder);
            protectedGroup.MapDelete("{id:guid}", DeleteOrder);
            protectedGroup.MapDelete("{id:Guid}/{itemId:Guid}", DeleteOrderItem);
        }



        private static async Task<IResult> GetOrders(
            ClaimsPrincipal user,
            [FromServices] OrderContext context)
        {
            var orders = await context.Orders.ToListAsync();
            return TypedResults.Ok(orders);
        }

        private static async Task<IResult> GetOrderById(
            ClaimsPrincipal user,
            [FromRoute] Guid id,
            [FromServices] OrderContext context)
        {
            var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            return TypedResults.Ok(order);
        }


        private static async Task<IResult> InsertOrder(
            ClaimsPrincipal user,
            [FromBody] InsertOrderRequest request,
            [FromServices] IValidator<InsertOrderRequest> validator,
            [FromServices] OrderContext context
        )
        {

            var userId = ClaimsHelper.GetUserId(user);

            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }


            var order = new CostumerOrder
            {
                Id = Guid.NewGuid(),
                BuyerId = Guid.Parse(userId),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
            };

            var orderItems = new List<OrderItem>();
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,

                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                orderItems.Add(orderItem);
            }

            order.Items = orderItems;

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return Results.Created($"/order/{order.Id}", order);
        }

        private static async Task<IResult> UpdateOrder(
            ClaimsPrincipal user,
            [FromRoute] Guid id,
            [FromBody] OrderItemRequest request,
            [FromServices] IValidator<OrderItemRequest> validator,
            [FromServices] OrderContext context)
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

            var existingOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (existingOrder == null)
            {
                return Results.NotFound();
            }


            if (existingOrder.Status != OrderStatus.Pending)
            {
                return Results.BadRequest("Order is not in pending state");
            }

            var itemToUpdate = existingOrder.Items.FirstOrDefault(i => i.Id == request.Id);

            if (itemToUpdate == null)
            {
                return Results.NotFound();
            }

            itemToUpdate.Quantity = request.Quantity;

            await context.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        private static async Task<IResult> DeleteOrder(
            ClaimsPrincipal user,
            [FromRoute] Guid id,
            [FromServices] OrderContext context)
        {
            var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return TypedResults.NotFound();
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        private static async Task<IResult> DeleteOrderItem(
            ClaimsPrincipal user,
            [FromRoute] Guid id,
            [FromRoute] Guid itemId,
            [FromServices] OrderContext context
        )
        {
            var buyer = ClaimsHelper.GetUserId(user);

            if (string.IsNullOrEmpty(buyer))
            {
                return Results.Unauthorized();
            }

            var buyerId = Guid.Parse(buyer);

            var order = await context.Orders.FirstOrDefaultAsync(o => o.BuyerId == buyerId && o.Id == id);

            if (order == null)
            {
                return Results.NotFound();
            }

            var orderItem = order.Items.FirstOrDefault(i => i.Id == itemId);

            if (orderItem == null)
            {
                return Results.NotFound();
            }

            order.Items.Remove(orderItem);
            await context.SaveChangesAsync();
            return TypedResults.NoContent();  

        }
    }
}
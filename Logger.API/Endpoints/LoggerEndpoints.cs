
using FluentValidation;
using Logger.API.Data;
using Logger.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class LoggerEndpoints
{
    public static void MapLoggerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/logger/ping", () => "pong");
        app.MapGet("/logger/logs", ListMessages);
        app.MapGet("/logger/logs/{service}", ListByService);
        app.MapDelete("/logger/clear", ClearMessages);
    }



    private static async Task<IResult> ListMessages(
        [FromServices] LoggerContext context
    )
    {
        var messages = await context.Messages
                .Include(m => m.Consumers)
                .ToListAsync();
        return Results.Ok(messages);    
    }


    private static async Task<IResult> ListByService(
        HttpContext httpContext,
        [FromServices] IValidator<ServiceSelectorRequest> validator,
        [FromServices] LoggerContext context )
    {
        var query = httpContext.Request.Query;

        var request = ServiceSelectorRequest.FromQuery(query);

        if (request == null)
        {
            return Results.BadRequest("The query contains empty or malformed values.");
        }

        var validatonResult = validator.Validate(request);

        if (!validatonResult.IsValid)
        {
            return Results.BadRequest(validatonResult.ToDictionary());
        }

        var messages = await context.Messages.Where(m => m.Service == request.Service).ToListAsync();

        return Results.Ok(messages);
    }


    private static async  Task<IResult> ClearMessages(
        [FromServices] LoggerContext context
    )
    {
         // Remove all rows from the Messages table
        context.Messages.RemoveRange(await context.Messages.ToListAsync());

        // Save changes to persist the deletion
        await context.SaveChangesAsync();

        return Results.Ok("All messages cleared successfully.");
    }
        
}
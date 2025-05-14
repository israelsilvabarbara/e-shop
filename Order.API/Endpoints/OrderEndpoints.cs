
namespace Order.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var publicGroup = app.MapGroup("/order");
            var protectedGroup = app.MapGroup("/order").RequireAuthorization();

            publicGroup.MapGet("/health", () => "ok");
        }
    }
}
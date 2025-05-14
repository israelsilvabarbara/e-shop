namespace Basket.API.DTOs
{
    public record UpdateBasketItemRequest
    (
        Guid ProductId,
        int Quantity
    );
}
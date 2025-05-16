namespace Basket.API.DTOs
{
    public record UpdateBasketItemRequest
    (
        Guid ItemId,
        int Quantity
    );
}
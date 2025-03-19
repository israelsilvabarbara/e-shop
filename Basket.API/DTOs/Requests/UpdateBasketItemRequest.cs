namespace Basket.API.DTOs
{
    public record UpdateBasketItemRequest
    (
        string BasketId,
        int ProductId,
        int Quantity
    );
}
namespace Basket.API.DTOs
{
    public record UpdateBasketItemRequest
    (
        string BuyerId,
        int ProductId,
        int Quantity
    );
}
namespace Basket.API.DTOs
{
    public record  BasketItemRequest
    (
        string BuyerId,
        int ProductId 
    );
}
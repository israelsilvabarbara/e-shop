namespace Basket.API.DTOs
{
    public record CreateBasketItemRequest
    (
        string BuyerId,
        int ProductId, 
        string ProductName,
        int Quantity, 
        decimal UnitPrice, 
        string PictureUrl
    );
}
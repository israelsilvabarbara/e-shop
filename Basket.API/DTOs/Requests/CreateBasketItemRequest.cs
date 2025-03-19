namespace Basket.API.DTOs
{
    public record CreateBasketItemRequest
    (
        string BasketId,
        int ProductId, 
        string ProductName,
        int Quantity, 
        decimal UnitPrice, 
        string PictureUrl
    );
}
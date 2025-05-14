namespace Basket.API.DTOs
{
    public record CreateBasketItemRequest
    (
        Guid ProductId, 
        string ProductName,
        int Quantity, 
        decimal UnitPrice, 
        string PictureUrl
    );
}
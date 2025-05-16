namespace Basket.API.DTOs
{
    public record CreateBasketItemRequest
    (
        Guid ItemId, 
        string ItemName,
        int Quantity, 
        decimal UnitPrice, 
        string PictureUrl
    );
}
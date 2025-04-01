namespace Inventory.API.DTOs
{
    public record CreateItemRequest
    (
        Guid ProductId, 
        string ProductName, 
        int Stock, 
        int StockThreshold
    );
}
namespace Inventory.API.DTOs
{
    public record CreateItemRequest
    (
        int ProductId, 
        string ProductName, 
        int Stock, 
        int StockTresholdMin, 
        int StockTresholdMax
    );
}
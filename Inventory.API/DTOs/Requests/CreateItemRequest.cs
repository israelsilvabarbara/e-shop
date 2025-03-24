namespace Inventory.API.DTOs
{
    public record CreateItemRequest
    (
        string ProductName, 
        int Stock, 
        int StockTresholdMin, 
        int StockTresholdMax
    );
}
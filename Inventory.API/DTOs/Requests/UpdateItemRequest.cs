namespace Inventory.API.DTOs
{
    public record UpdateItemRequest
    (
        int ProductId,
        string? ProductName,
        int? Stock,
        int? StockTresholdMin,
        int? StockTresholdMax
    );
}
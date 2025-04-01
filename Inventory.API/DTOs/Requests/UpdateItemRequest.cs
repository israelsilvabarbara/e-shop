namespace Inventory.API.DTOs
{
    public record UpdateItemRequest
    (
        Guid ProductId,
        string? ProductName,
        int? Stock,
        int? StockTreshold
    );
}
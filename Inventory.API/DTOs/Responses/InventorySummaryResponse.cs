namespace Inventory.API.DTOs
{
    public record InventorySummaryResponse
    (
        Guid ProductId,
        int Stock
    );
}
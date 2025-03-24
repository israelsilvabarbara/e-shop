namespace Inventory.API.DTOs
{
    public record InventorySummaryResponse
    (
        int ProductId,
        int Stock
    );
}
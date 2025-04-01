namespace Inventory.API.DTOs
{
    public record InventoryDetailResponse
    (
        Guid ProductId,
        int Stock
    );
}
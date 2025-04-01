namespace Inventory.API.DTOs
{
    public record ListInventoryResponse(
        int Count,
        IEnumerable<InventoryDetailResponse> Items
    );
}
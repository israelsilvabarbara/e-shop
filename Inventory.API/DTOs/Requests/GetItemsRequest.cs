namespace Inventory.API.DTOs
{
    public record GetItemsRequest(
        List<Guid> ProductIds
    );
}
namespace Inventory.API.DTOs
{
    public record GetItemsRequest(
        List<int> ProductIds
    );
}
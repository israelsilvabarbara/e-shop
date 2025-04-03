namespace Inventory.API.DTOs
{
    public record RestockItemRequest
    (
        Guid ProductId,
        int Quantity
    );
}
namespace Inventory.API.DTOs
{
    public record RestockResponse
    (
        Guid ProductId,
        string ProductName,
        int Stock
    );
}
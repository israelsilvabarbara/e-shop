namespace Order.API.DTOs
{
    public record OrderItemRequest
    (
        Guid Id,
        string Name,
        decimal UnitPrice,
        int Quantity
    );
}
namespace Order.API.DTOs
{
    public record InsertOrderRequest
    (
        IEnumerable<OrderItemRequest> Items
    );


}
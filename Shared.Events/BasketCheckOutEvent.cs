namespace Shared.Events
{
    public record BasketCheckoutEvent
    (
        Guid Id,
        Guid BasketId,
        Guid BuyerId,
        IEnumerable<ItemDetails> Items,
        DateTime EventDate
    ) : IEvent;


    public record ItemDetails
    (
        Guid Id,
        string Name,
        int Quantity,
        decimal UnitPrice
    );
}
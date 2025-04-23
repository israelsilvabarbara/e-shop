namespace Shared.Events
{
    public record DiscountApplied
    (
        Guid Id,
        int OrderId,
        int Discount,
        DateTime EventDate
    ):BaseEvent(Id);
}
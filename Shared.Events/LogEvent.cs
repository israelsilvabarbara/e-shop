namespace Shared.Events
{
    public record LogEvent
    (
        Guid Id,    
        DateTime Timestamp, 
        string Service,   
        string EventType,   
        string Status,   
        string? Details 
    ):IEvent;
}
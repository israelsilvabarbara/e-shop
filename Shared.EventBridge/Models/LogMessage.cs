public class LogMessage
{
    public Guid Id { get; set; }
    public string Service { get; set; }
    public DateTime Timestamp { get; set; }
    public LogEventType EventType { get; set; }
    public LogStatus Status { get; set; }
    public string Details { get; set; }
}

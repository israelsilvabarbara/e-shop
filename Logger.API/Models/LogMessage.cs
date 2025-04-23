namespace Logger.API.Models
{
    public class LogMessage
    {
        public Guid Id { get; set; }
        public required string Service { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string EventType { get; set; }
        public required string Status { get; set; }
        public string Details { get; set; } = "";

        public ICollection<LogConsumer> Consumers { get; set; } = new List<LogConsumer>();
    }
}


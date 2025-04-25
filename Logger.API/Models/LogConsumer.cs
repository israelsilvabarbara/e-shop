namespace Logger.API.Models
{
    public class LogConsumer
    {
        public Guid Id {get; set;}
        public Guid LogMessageId {get; set;}
        public required string ConsumerService {get; set;}
        public required DateTime ConsumedTime {get; set;}
        public string? Details { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public LogMessage? LogMessage {get; set;}
    }
}
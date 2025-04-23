using Logger.API.Data;
using Logger.API.Models;
using MassTransit;
using Shared.Events;

namespace Logger.API.EventBus
{
    public class LogEventConsumer : IConsumer<LogEvent>
    {

        private readonly LoggerContext _dbContext;

        public LogEventConsumer(LoggerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<LogEvent> context)
        {
            var message = context.Message;
            Console.WriteLine($"Received log event: {message.Id}");

            var LogMessage = new LogMessage
            {
                Id = message.Id,
                Timestamp = message.Timestamp,
                Service = message.Service,
                EventType = message.EventType,
                Status = message.Status,
                Details = !string.IsNullOrEmpty(message.Details) ? message.Details : "",
            };

            await _dbContext.Messages.AddAsync(LogMessage);

            await _dbContext.SaveChangesAsync();

        }
    }
}
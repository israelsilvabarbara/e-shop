using System.Text.Json;
using Logger.API.Data;
using Logger.API.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Logger.API.EventBus
{
    public class LogConsumedEventConsumer : IConsumer<LogConsumedEvent>
    {
        private readonly LoggerContext _dbContext;

        public LogConsumedEventConsumer(LoggerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<LogConsumedEvent> context)
        {
            var message = context.Message;

            if (message == null)
            {
                return;
            }

            const int maxRetries = 3;
            LogMessage? origin = null;
            for (int i = 0; i < maxRetries; i++)
            {
                origin = await _dbContext.Messages.FirstOrDefaultAsync(m => m.EventId == message.Id);
                if (origin != null)
                    break;
                await Task.Delay(100); // Delay for 100ms before retrying
            }

            
            if (origin == null)
            {   
                return;
            }

            var consumer = new LogConsumer
            {
                Id = Guid.NewGuid(),
                LogMessageId = origin.Id,
                ConsumerService = message.Service,
                ConsumedTime = message.Timestamp,
                Details = !string.IsNullOrEmpty(message.Details) ? message.Details : "",
            };

            await _dbContext.Consumers.AddAsync(consumer);

            await _dbContext.SaveChangesAsync();
        
        }
    }
}
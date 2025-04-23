using Logger.API.Data;
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

            var origin = await _dbContext.Messages.FirstOrDefaultAsync(m => m.Id == message.Id);

            if (origin == null)
            {   
                return;
            }

            





        }
    }
}
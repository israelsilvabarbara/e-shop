using Identity.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Enums;
using Shared.Events;

namespace Identity.API.EventConsumers
{
    public class IdentityKeyGeneratedEventConsumer : IConsumer<IdentityKeyGeneratedEvent>
    {
        private readonly IdentityContext _dbContext;
        private readonly EventBus _eventBus;

        public IdentityKeyGeneratedEventConsumer( IdentityContext dbContext, 
                                                  EventBus eventBus )
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task Consume(ConsumeContext<IdentityKeyGeneratedEvent> context)
        {

            var message = context.Message;
            
            // update internal key pairs
            var keyVault = await _dbContext.KeyVaults.FirstOrDefaultAsync();

            if( keyVault == null )
            {
                Console.WriteLine("Error: No key vault found.");
                return;
            }

            // publish event with new public key to other services

            var keyEvent = new IdentityRefreshAuthorizationEvent
            (
                Id: Guid.NewGuid(),
                PublicKey: keyVault.PublicKey,
                Expiration: keyVault.Expiration,
                EventDate: DateTime.UtcNow
            );

            await _eventBus.SendAsync( keyEvent, 
                                    Services.Identity, 
                                    LogEventType.Info, 
                                    LogStatus.Success, 
                                    "Key updated and sent to other services."
                );
        }
    }
}

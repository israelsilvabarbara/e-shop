using Identity.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Inventory.API.EventBus
{
    public class IdentityKeyGeneratedEventConsumer : IConsumer<IdentityKeyGeneratedEvent>
    {
        private readonly IdentityContext _dbContext;
        private readonly IPublishEndpoint _eventBus;

        public IdentityKeyGeneratedEventConsumer( IdentityContext dbContext, 
                                                  IPublishEndpoint eventBus )
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
                PublicKey: keyVault.PublicKey,
                Expiration: keyVault.Expiration,
                EventDate: DateTime.UtcNow
            );

            await _eventBus.Publish(keyEvent);
        }
    }
}

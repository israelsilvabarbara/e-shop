using Identity.KeyGen.Service.Data;
using Identity.KeyGen.Service.Models;
using Identity.KeyGen.Service.Services;
using Microsoft.EntityFrameworkCore;
using Shared.EventBridge.Enums;
using Shared.Events;

namespace Identity.KeyGen.Service
{
    public class KeyUpdateExecutor
    {
        private readonly IdentityContext _dbContext;
        private readonly EventBus _eventBus;
        private readonly KeyGenerator _keyGenerator;
        public KeyUpdateExecutor(IdentityContext dbContext, EventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _keyGenerator = new KeyGenerator();
        }

        private bool IsCronStarted() => 
            Environment.GetEnvironmentVariable("CRON_STARTED")?.ToLower() == "true";
        public async Task ExecuteAsync()
        {    
            if ( !IsCronStarted() )
            {
                var tableWithContent = await _dbContext.KeyVaults.AnyAsync();
                if( tableWithContent )
                {
                    return;
                }
            }

            // Generate new keys
            var keyPair = _keyGenerator.GenerateKeyPair();                

            // Save new keys to the database
            var newKeyVault = new KeyVault
            {
                PrivateKey = keyPair.PrivateKey,
                PublicKey = keyPair.PublicKey,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(7)
            };
            await _dbContext.KeyVaults.AddAsync(newKeyVault);
            await _dbContext.SaveChangesAsync();

            // Publish an event to Identity.API
            var keyEvent = new IdentityKeyGeneratedEvent
            (
                Id: Guid.NewGuid(),
                EventDate: DateTime.UtcNow
            );

            await _eventBus.SendAsync( keyEvent, 
                                    Shared.EventBridge.Enums.Services.Identity, 
                                    LogEventType.Info, 
                                    LogStatus.Success, 
                                    "New KeyPair Generated.");

        }
    }
}

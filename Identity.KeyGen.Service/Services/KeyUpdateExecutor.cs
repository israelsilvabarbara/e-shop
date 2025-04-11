using Identity.KeyGen.Service.Data;
using Identity.KeyGen.Service.Models;
using Identity.KeyGen.Service.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Identity.KeyGen.Service
{
    public class KeyUpdateExecutor
    {
        private readonly IdentityContext _dbContext;
        private readonly KeyGenerator _keyGenerator;
        private readonly IPublishEndpoint _publishEndpoint; 
        public KeyUpdateExecutor(IdentityContext dbContext,  IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _keyGenerator = new KeyGenerator();
        }

        private bool IsCronStarted() => 
            Environment.GetEnvironmentVariable("CRON_STARTED")?.ToLower() == "true";
        public async Task ExecuteAsync()
        {    
            if ( !IsCronStarted() )
            {
                Console.WriteLine("Service not started by cron.");

                Console.WriteLine("Waiting 30seconds to start the database");
                await Task.Delay(30000);
                var tableWithRows = await _dbContext.KeyVaults.AnyAsync();
                if (tableWithRows )
                {
                    Console.WriteLine("Keys already exist in the database.");
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

            Console.WriteLine("Keys updated and saved to the database.");

            // Publish an event to Identity.API
            await _publishEndpoint.Publish(new IdentityKeyGeneratedEvent
            (
                EventDate: DateTime.UtcNow
            ));

            Console.WriteLine("Key update event published to Identity.API.");
        }
    }
}

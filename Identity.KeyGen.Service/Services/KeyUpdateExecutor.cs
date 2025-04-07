using Identity.KeyGen.Service.Data;
using Identity.KeyGen.Service.Models;
using Identity.KeyGen.Service.Services;
using MassTransit;
using Shared.Events;

namespace Identity.KeyGen.Service.Execution
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

        public async Task ExecuteAsync()
        {
            // Check if the table is empty
            var existingKeys = _dbContext.KeyVaults.FirstOrDefault();
            if (existingKeys != null)
            {
                Console.WriteLine("Keys already exist. No update required.");
                return;
            }

            // Check if the service is started by a cron job
            var isCronStarted = Environment.GetEnvironmentVariable("CRON_STARTED");
            if (isCronStarted == null || isCronStarted != "true")
            {
                Console.WriteLine("Service not started by cron. Exiting...");
                return;
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

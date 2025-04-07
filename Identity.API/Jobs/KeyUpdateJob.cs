/* using Identity.API.Data;
using Identity.API.Utils;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Identity.API.Jobs
{
    public class KeyUpdateJob : IJob
    {
        private readonly IdentityContext _context;

        public KeyUpdateJob(IdentityContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Fetch the current keys from the database
            var currentKeyVault = await _context.KeyVaults.FirstOrDefaultAsync();
            if (currentKeyVault == null)
            {
                // No existing keys, generate and add new ones
                var newKeys = KeyUtils.GenerateKeys();
                currentKeyVault = new Models.KeyVault
                {
                    PrivateKey = newKeys.PrivateKey,
                    PublicKey = newKeys.PublicKey,
                    CreatedAt = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    Version = Guid.NewGuid().ToString()
                };
                await _context.KeyVaults.AddAsync(currentKeyVault);
            }
            else
            {
                // Check if keys need to be updated
                if (DateTime.UtcNow >= currentKeyVault.CreatedAt.AddDays(7))
                {
                    var newKeys = KeyUtils.GenerateKeys();
                    currentKeyVault.PrivateKey = newKeys.PrivateKey;
                    currentKeyVault.PublicKey = newKeys.PublicKey;
                    currentKeyVault.CreatedAt = DateTime.UtcNow;
                    currentKeyVault.Expiration = DateTime.UtcNow.AddDays(7);
                    currentKeyVault.Version = Guid.NewGuid().ToString();
                    _context.KeyVaults.Update(currentKeyVault);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
 */
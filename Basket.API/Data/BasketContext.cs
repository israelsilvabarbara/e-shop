using MongoDB.Driver;

namespace Basket.API.Data
{
    public class BasketContext
    {
        private readonly IMongoDatabase _database;

        public BasketContext(IConfiguration configuration)
        {
            // Build the connection string from environment variables
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "27017";
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "BasketDB";

            var connectionString = string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPass)
                ? $"mongodb://{dbHost}:{dbPort}" // No authentication
                : $"mongodb://{dbUser}:{dbPass}@{dbHost}:{dbPort}";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        // Access the "Baskets" collection
        public IMongoCollection<Models.Basket> Baskets => _database.GetCollection<Models.Basket>("Baskets");
    }
}

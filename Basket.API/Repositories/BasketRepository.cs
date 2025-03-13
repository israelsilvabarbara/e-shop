public class BasketRepository
{
    private readonly IMongoCollection<Basket> _baskets;

    public BasketRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("BasketDb"));
        var database = client.GetDatabase("BasketDb");
        _baskets = database.GetCollection<Basket>("Baskets");
    }
}

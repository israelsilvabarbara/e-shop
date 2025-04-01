public class InventoryService
{
    private readonly int _defaultThreshold;

    public InventoryService(IConfiguration configuration)
    {
        _defaultThreshold = configuration.GetValue<int>("DefaultThreshold");
    }

    public int DefaultThreshold => _defaultThreshold;
}

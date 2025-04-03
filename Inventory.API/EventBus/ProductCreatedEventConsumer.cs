using Inventory.API.Data;
using Inventory.API.Models;
using MassTransit;
using Shared.Events;

namespace Inventory.API.EventBus
{
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly InventoryContext _dbContext;
        private readonly int _InventoryTreshold;

        public ProductCreatedEventConsumer( InventoryContext dbContext, 
                                            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _InventoryTreshold = configuration.GetValue<int>("InventoryTreshold", 10);
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {

            var message = context.Message;
            
            var product = new InventoryItem
            {
                Id = Guid.NewGuid(),
                ProductId = message.ProductId,
                ProductName = message.ProductName,
                Stock = 0,
                StockThreshold = _InventoryTreshold
            };

            _dbContext.Inventorys.Add(product);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"Product Inventory created: {product.ProductId} - {product.ProductName}");
        }
    }
}

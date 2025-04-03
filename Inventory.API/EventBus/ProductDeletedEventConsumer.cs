using Inventory.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;

namespace Inventory.API.EventBus
{
    public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly InventoryContext _dbContext;

        public ProductDeletedEventConsumer(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task Consume( ConsumeContext<ProductDeletedEvent> context )
        {
            var message = context.Message;

            var item = await _dbContext.Inventorys.FirstOrDefaultAsync(i => i.ProductId == message.ProductId);

            if (item == null)
            {
                return;
            }

            _dbContext.Inventorys.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
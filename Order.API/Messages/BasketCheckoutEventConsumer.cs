

using MassTransit;
using Order.API.Data;
using Order.API.Models;
using Shared.EventBridge.Enums;
using Shared.Events;

namespace Order.API.Messages
{

    public class BasketCheckoutEventConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly OrderContext _dbContext;
        private readonly EventBus _eventBus;

        public BasketCheckoutEventConsumer(OrderContext dbContext,
                                            EventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;
            await _eventBus.ConsumeAsync(message.Id, Services.Order, "Order created");

            var order = new CostumerOrder
            {
                Id = Guid.NewGuid(),
                BuyerId = message.BuyerId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
            };

            var orderItems = new List<OrderItem>();
            foreach (var item in message.Items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,

                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                orderItems.Add(orderItem);
            }

            order.Items = orderItems;

        

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

     
    }
}
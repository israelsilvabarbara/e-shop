

namespace Order.API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public required string  Name { get; set; }
        public required int     Quantity { get; set; }
        public required decimal UnitPrice { get; set; }

        
        public CostumerOrder Order { get; set; } //EF navigation property
    }
}
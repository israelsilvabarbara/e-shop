

namespace Order.API.Models
{
    public class CostumerOrder
    {
        public Guid Id { get; set; }
        public Guid BuyerId  { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}


namespace Order.API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public CostumerOrder Order { get; set; }

        public string  ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int     Quantity { get; set; }
    }
}
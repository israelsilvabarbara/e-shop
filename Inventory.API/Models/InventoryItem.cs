using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.API.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public required Guid Id { get; set; }

        [BsonElement("productId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProductId { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("stockThreshold")]
        public int StockThreshold { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.API.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;} = string.Empty;
        
        [BsonElement("productId")]
        public Guid ProductId {get; set;}
        
        [BsonElement("productName")]
        public required string ProductName {get; set;}
        
        [BsonElement("stock")]
        public int Stock {get; set;}
        
        [BsonElement("stockTreshold")]
        public int StockTreshold {get; set;}

    }
}
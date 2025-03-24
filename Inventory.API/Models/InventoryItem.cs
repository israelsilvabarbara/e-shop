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
        public int ProdutctId {get; set;}
        
        [BsonElement("productName")]
        public required string ProductName {get; set;}
        
        [BsonElement("stock")]
        public int Stock {get; set;}
        
        [BsonElement("stockTresholdMin")]
        public int StockTresholdMin {get; set;}
        
        [BsonElement("stockTresholdMax")]
        public int StockTresholdMax {get; set;}
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Basket.API.Models
{
    public class BasketSelection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("buyerId")]
        public required string BuyerId { get; set; }
        
        [BsonElement("items")]
        public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}

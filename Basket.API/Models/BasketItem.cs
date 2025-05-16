using MongoDB.Bson.Serialization.Attributes;

namespace Basket.API.Models
{
    public class BasketItem 
    {
        [BsonElement("itemId")]
        public required Guid ItemId { get; set; }

        [BsonElement("itemName")]
        public required string ItemName { get; set; }

        [BsonElement("quantity")]
        public required int Quantity { get; set; }

        [BsonElement("unitPrice")]
        public required decimal UnitPrice { get; set; }

        [BsonElement("pictureUrl")]
        public required string PictureUrl { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;

namespace Basket.API.Models
{
    public class BasketItem 
    {
        [BsonElement("productId")]
        public required int ProductId { get; set; }

        [BsonElement("productName")]
        public required string ProductName { get; set; }

        [BsonElement("quantity")]
        public required int Quantity { get; set; }

        [BsonElement("unitPrice")]
        public required decimal UnitPrice { get; set; }

        [BsonElement("pictureUrl")]
        public required string PictureUrl { get; set; }
    }
}


namespace Basket.API.Models
{

    public class BasketItem 
    {
        public required int     ProductId    { get; set; }
        public required string  ProductName  { get; set; }
        public required int     Quantity     { get; set; }
        public required decimal UnitPrice    { get; set; }
        public required string  PictureUrl   { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Models
{
    public class BasketOwner
    {
        [Key]
        public int Id { get; set; }
        public string BuyerId { get; set; }

        public List<BasketItem> Items { get; set; } = [];
    }
}

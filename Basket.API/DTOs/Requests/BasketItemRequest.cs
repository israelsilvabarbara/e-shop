public class BasketItemRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
}

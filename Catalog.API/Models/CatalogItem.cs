namespace Catalog.API.Models
{
    
public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public string PictureFileName { get; set; } = String.Empty;
    public string PictureUri { get; set; } = String.Empty;
    public int CatalogTypeId { get; set; }
    public int CatalogBrandId { get; set; }
    public int AvailableStock { get; set; }
    public int RestockThreshold { get; set; }
    public int MaxStockThreshold { get; set; }
}
}
 
namespace Catalog.API.Models
{
    public class CatalogType
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
        public ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>(); // Navigation property  
    }
}
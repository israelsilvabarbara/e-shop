namespace Catalog.API.Models
{
    public class CatalogBrand
    {
        public Guid Id { get; set; }
        public required string Brand { get; set; }

        public ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>(); // Navigation property
    }
}
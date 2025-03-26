using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    
    public class CatalogItem
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string PictureFileName { get; set; } = String.Empty;
        public string PictureUri { get; set; } = String.Empty;
        public decimal Price { get; set; }
        
        public required Guid CatalogBrandId { get; set; } // Foreign Key
        public CatalogBrand CatalogBrand { get; set; } = null!; // Navigation Property
    
        public required Guid CatalogTypeId { get; set; } // Foreign Key
        public CatalogType CatalogType { get; set; } = null!;// Navigation Property
        
    }
}
 
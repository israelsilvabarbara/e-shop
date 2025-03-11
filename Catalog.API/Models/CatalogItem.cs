using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    
    public class CatalogItem
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string PictureFileName { get; set; } = String.Empty;
        public string PictureUri { get; set; } = String.Empty;
        public decimal Price { get; set; }

        public int CatalogBrandId { get; set; } // Foreign Key
        public CatalogBrand? CatalogBrand { get; set; } // Navigation Property
    
        public int CatalogTypeId { get; set; } // Foreign Key
        public CatalogType? CatalogType { get; set; } // Navigation Property
        
        public int AvailableStock { get; set; }
        public int RestockThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
    }
}
 
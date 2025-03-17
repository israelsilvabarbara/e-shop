using System.ComponentModel;

namespace Catalog.API.DTOs{

    public record FilterRequest(
        [property: Description("Filter Items With Price Higher Than")]
        int? minPrice,

        [property: Description("Filter Items With Price Lower Than")]
        int? maxPrice,

        [property: Description("Filter Items With Catalog Type")]
        string? CatalogType, 
        
        [property: Description("Filter Items With Catalog Brand")]
        string? CatalogBrand
    );
}

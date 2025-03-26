
using System.ComponentModel;

namespace Catalog.API.DTOs
{
    public record CreateItemRequest(
        [property: Description("The name of the item")]
        string Name,
        
        [property: Description("The description of the item")]
        string Description,

        [property: Description("The picture file name of the item")]
        string PictureFileName,
        
        [property: Description("The price of the item")]
        decimal Price,
        
        [property: Description("The catalog brand id of the item")]
        Guid CatalogBrandId,
        
        [property: Description("The catalog type id of the item")]
        Guid CatalogTypeId
    );
}

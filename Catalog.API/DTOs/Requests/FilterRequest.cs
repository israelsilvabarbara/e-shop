
using System.ComponentModel;

namespace Catalog.API.DTOs{

    public record FilterRequest(
        [property: Description("Filter Items With Price Higher Than")]
        int? MinPrice,

        [property: Description("Filter Items With Price Lower Than")]
        int? MaxPrice,

        [property: Description("Filter Items With Catalog Type")]
        string? Type, 
        
        [property: Description("Filter Items With Catalog Brand")]
        string? Brand
    );
}

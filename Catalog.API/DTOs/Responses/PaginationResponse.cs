
namespace Catalog.API.DTOs{

    public record PaginationResponse<T>
    ( 
        int TotalCount,
        PaginationRequest PageDetails,
        FilterRequest FilterDetails,
        IEnumerable<T> Items
    );
}

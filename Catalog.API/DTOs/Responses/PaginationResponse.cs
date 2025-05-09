
namespace Catalog.API.DTOs{

    public record PaginationResponse<T>
    ( 
        int TotalCount,
        IEnumerable<T> Items
    );
}

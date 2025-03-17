
namespace Catalog.API.DTOs{

    public record PaginationResponse<T>
    ( 
        IEnumerable<T> Items,
        int TotalCount
    );
}

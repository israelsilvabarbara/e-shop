namespace Catalog.API.DTOs
{
    public record ItemListRequest(
        PaginationRequest Pagination,
        FilterRequest Filter
    );
}
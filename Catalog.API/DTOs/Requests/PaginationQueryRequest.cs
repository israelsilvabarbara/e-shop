namespace Catalog.API.DTOs
{
    public record PaginationQueryRequest
    (
        int Number = 1,
        int Size = 30
    );
}
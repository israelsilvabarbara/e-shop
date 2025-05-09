namespace Catalog.API.DTOs
{
    public record FilterQueryRequest
    (
        int? MinPrice,
        int? MaxPrice,
        string? Type,
        string? Brand
    );
}
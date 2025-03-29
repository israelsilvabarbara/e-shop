namespace Catalog.API.DTOs
{
    public record InsertBrandRequest
    (
        IEnumerable<string> Brands
    );
}
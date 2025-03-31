namespace Catalog.API.DTOs
{
    public record ListBrandsResponse
    (
        int Count,
        List<ItemResponse> Brands
    );
}
namespace Catalog.API.DTOs
{
    public record ListTypesResponse
    (
        int Count,
        List<ItemResponse> Types
    );
}
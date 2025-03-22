namespace Catalog.API.DTOs
{
    public record UpdateItemRequest
    (
        int Id,
        string? Name,
        string? Description,
        string? PictureFileName,
        string? PictureUrl,
        decimal? Price,
        string? Brand,
        string? Type
    );
}
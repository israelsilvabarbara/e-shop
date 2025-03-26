namespace Catalog.API.DTOs
{
    public record UpdateItemRequest
    (
        Guid Id,
        string? Name,
        string? Description,
        string? PictureFileName,
        string? PictureUrl,
        decimal? Price,
        string? Brand,
        string? Type
    );
}
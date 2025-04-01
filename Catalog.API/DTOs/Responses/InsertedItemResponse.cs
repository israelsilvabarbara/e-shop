namespace Catalog.API.DTOs
{
    public record InsertedItemResponse(
        Guid Id,
        string Name,
        string Description,
        string PictureFileName,
        string PictureUri,
        decimal Price,
        Guid CatalogBrandId,
        Guid CatalogTypeId
    );
}
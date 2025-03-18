
namespace Catalog.API.DTOs
{
    public record CreateItemRequest(
        string Name,
        string Description,
        string PictureFileName,
        decimal Price,
        int CatalogBrandId,
        int CatalogTypeId,
        int AvailableStock,
        int RestockThreshold,
        int MaxStockThreshold
    );
}

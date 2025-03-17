using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs
{
    public record CreateItemRequest(
        [Required(ErrorMessage = "Name is required.")]
        string Name,

        [Required(ErrorMessage = "Description is required.")]
        string Description,

        [Required(ErrorMessage = "PictureFileName is required.")]
        string PictureFileName,

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        decimal Price,

        [Range(1, int.MaxValue, ErrorMessage = "CatalogBrandId must be a valid ID.")]
        int CatalogBrandId,

        [Range(1, int.MaxValue, ErrorMessage = "CatalogTypeId must be a valid ID.")]
        int CatalogTypeId,

        [Range(0, int.MaxValue, ErrorMessage = "AvailableStock must be 0 or higher.")]
        int AvailableStock,

        [Range(0, int.MaxValue, ErrorMessage = "RestockThreshold must be 0 or higher.")]
        int RestockThreshold,

        [Range(0, int.MaxValue, ErrorMessage = "MaxStockThreshold must be 0 or higher.")]
        int MaxStockThreshold
    );
}

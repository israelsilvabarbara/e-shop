namespace Catalog.API.DTOs
{
    public record InsertTypeRequest
    (
        IEnumerable<string> Types
    );
}
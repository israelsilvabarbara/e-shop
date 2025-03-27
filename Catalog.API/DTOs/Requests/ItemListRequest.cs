namespace Catalog.API.DTOs
{
    public record ItemListRequest(
        PaginationRequest Pagination,
        FilterRequest Filter
    )
    {
        public static ItemListRequest? FromQuery(IQueryCollection query)
        {
            try
            {
                // Parse FilterRequest
                var filter = new FilterRequest(
                    MinPrice: query.ContainsKey("MinPrice") && int.TryParse(query["MinPrice"], out var min) && min >= 0 ? min : null,
                    MaxPrice: query.ContainsKey("MaxPrice") && int.TryParse(query["MaxPrice"], out var max) && max >= 0 ? max : null,
                    Type: query.ContainsKey("Type") && !string.IsNullOrWhiteSpace(query["Type"]) ? query["Type"].ToString() : null,
                    Brand: query.ContainsKey("Brand") && !string.IsNullOrWhiteSpace(query["Brand"]) ? query["Brand"].ToString() : null
                );

                // Parse PaginationRequest
                var pagination = new PaginationRequest(
                    PageNumber: query.ContainsKey("PageNumber") && int.TryParse(query["PageNumber"], out var pageNumber) && pageNumber > 0 ? pageNumber : 1,
                    PageSize: query.ContainsKey("PageSize") && int.TryParse(query["PageSize"], out var pageSize) && pageSize > 0 ? pageSize : 10
                );

                return new ItemListRequest(pagination,filter);
            }
            catch
            {
                // Return null if any parsing fails
                return null;
            }
        }
    }
}
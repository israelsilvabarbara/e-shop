public record PaginationResponse<T>
( 
    IEnumerable<T> Items,
    int TotalCount
);

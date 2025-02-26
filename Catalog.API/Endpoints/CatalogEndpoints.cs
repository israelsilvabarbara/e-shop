

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        app.MapGet("/api/Catalog/", () => "Hello World!");
    }
}
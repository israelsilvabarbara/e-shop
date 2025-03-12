

public static class CatalogEndPoints {

    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app) {
        app.MapGet("/api/Catalog/", () => "api/Catalog endpoint is working");
        app.MapGet("/", () => "root endpoint working");
    }
    
}
namespace Catalog.API.Features.Products.GetProducts;

public record GetProductsResponse(IReadOnlyList<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Get Products")
        .WithDescription("Get products");
    }
}
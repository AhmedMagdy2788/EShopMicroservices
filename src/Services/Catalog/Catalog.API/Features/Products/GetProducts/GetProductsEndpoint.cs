namespace Catalog.API.Features.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                var result = await sender.Send(query);
                return result.ToHttpResponse();
            })
            .WithName("GetProducts")
            .Produces<Result<IPagedList<Product>>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Products")
            .WithDescription("Get products");
    }
}
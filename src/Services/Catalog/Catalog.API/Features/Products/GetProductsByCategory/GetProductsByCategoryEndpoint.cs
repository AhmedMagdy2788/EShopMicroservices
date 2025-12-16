using Catalog.API.Features.Products.GetProducts;

namespace Catalog.API.Features.Products.GetProductsByCategory;

public record GetProductsByCategoryRequest(string Category, int? PageNumber = 1, int? PageSize = 10);

public record GetProductsByCategoryResponse(IPagedList<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category", async ([AsParameters]  GetProductsByCategoryRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsByCategoryQuery>();
                var result = await sender.Send(query);
                return result.ToHttpResponse();
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(statusCode: StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Products by Category")
            .WithDescription("Get products by Category");
    }
}
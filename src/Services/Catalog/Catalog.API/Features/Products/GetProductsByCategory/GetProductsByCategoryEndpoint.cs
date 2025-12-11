using Catalog.API.Features.Products.GetProductById;

namespace Catalog.API.Features.Products.GetProductByCategory;

public record GetProductsByCategoryResponse(IReadOnlyList<Product> Products);

public class GetProductsByCategoryEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{categoryId}", async (string categoryId, ISender sender) =>
            {
                var query = new GetProductsByCategoryQuery(categoryId);
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
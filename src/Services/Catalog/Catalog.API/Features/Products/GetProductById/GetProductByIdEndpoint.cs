namespace Catalog.API.Features.Products.GetProductById;

public record GetProductByIdRequest(Guid ProductId);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);

                // If success, return OK with the value
                // If failure, map error code to appropriate HTTP response
                return !result.IsSuccess
                    ? result.ToHttpResponse()
                    : Results.Ok(result);
            })
            .WithName("GetProductById")
            .Produces<Result<Product>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound) 
            .Produces(StatusCodes.Status400BadRequest) 
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Product by Id")
            .WithDescription("Get product by Id");
    }
}

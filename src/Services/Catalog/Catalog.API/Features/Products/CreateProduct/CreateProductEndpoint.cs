namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Categories,
    string Description,
    decimal Price,
    string ImageFile);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                if (!result.IsSuccess) return result.ToHttpResponse();
                return Results.Created($"/products/{result.Value.Id}", result);
            })
            .WithName("CreateProduct")
            .Produces<string>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a Product")
            .WithDescription("Create a product");
    }
}
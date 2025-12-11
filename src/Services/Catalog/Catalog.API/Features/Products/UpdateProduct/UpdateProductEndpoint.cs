namespace Catalog.API.Features.Products.UpdateProduct;

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:Guid}", async (Guid id, Product request, ISender sender) =>
            {
                var command = new UpdateProductCommand(
                    id,
                    request
                );

                var result = await sender.Send(command);
                return result.ToHttpResponse();
            })
            .WithName("UpdateProduct")
            .WithTags("Products")
            .Produces<Unit>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update Product")
            .WithDescription("Update product");
        ;
    }
}
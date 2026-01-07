namespace Basket.API.Features.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);
// public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/basket/{userName}",
                async (string userName, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteBasketCommand(userName));

                    return result.ToHttpResponse();
                }
            )
            .WithName("DeleteProduct")
            .Produces<Result<DeleteBasketResult>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
    }
}
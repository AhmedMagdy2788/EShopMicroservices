namespace Basket.API.Features.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public class StoreBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            return result.ToHttpResponse();
        }).WithName("StoreBasket")
        .Produces<Result<bool>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store basket cart")
        .WithDescription("Store basket cart ");
    }
}
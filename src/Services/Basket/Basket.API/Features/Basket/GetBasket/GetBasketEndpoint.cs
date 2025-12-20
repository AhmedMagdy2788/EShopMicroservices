namespace Basket.API.Features.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}", async (string username, ISender sender) =>
            {
                var query = new GetBasketQuery(username);
                var result = await sender.Send(query);
                return result.ToHttpResponse();
            }).WithName("GetShoppingCart")
            .Produces<Result<ShoppingCart>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Shopping cart")
            .WithDescription("Get Shopping cart");
    }
}
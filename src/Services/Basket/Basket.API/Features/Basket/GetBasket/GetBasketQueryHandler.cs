namespace Basket.API.Features.Basket.GetBasket;

public record GetBasketQuery(string Username) : IQuery<Result<ShoppingCart>>;

public class GetBasketQueryHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, Result<ShoppingCart>>
{
    public async Task<Result<ShoppingCart>> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasketAsync(query.Username, cancellationToken);
        if (shoppingCart is null)
            return Result<ShoppingCart>.Failure(
                Error.NotFound($"Shopping cart for user {query.Username} not found")
            );
        return Result<ShoppingCart>.Success(shoppingCart, $"Shopping cart for user: {query.Username} found");
    }
}
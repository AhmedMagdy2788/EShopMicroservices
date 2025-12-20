namespace Basket.API.Features.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<Result<ShoppingCart>>;

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(c => c.Cart).NotNull().WithMessage("Cart is required");
        RuleFor(c => c.Cart.UserName).NotEmpty().WithMessage("Username can not be empty");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, Result<ShoppingCart>>
{
    public async Task<Result<ShoppingCart>> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.StoreBasketAsync(command.Cart, cancellationToken);
        return Result<ShoppingCart>.Success(cart,"Basket updated");
    }
}
using Discount.Grpc;

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

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountClient)
    : ICommandHandler<StoreBasketCommand, Result<ShoppingCart>>
{
    public async Task<Result<ShoppingCart>> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);

        var cart = await repository.StoreBasketAsync(command.Cart, cancellationToken);
        return Result<ShoppingCart>.Success(cart, "Basket updated");
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        //communicate with Discount.Grpc service and calculate the latest price of products into sc
        foreach (var item in cart.Items)
        {
            var coupon =
                await discountClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
            item.Price -= coupon.Amount;
        }
    }
}
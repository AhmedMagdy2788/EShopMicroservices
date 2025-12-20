namespace Basket.API.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<Result<DeleteBasketResult>>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class DeleteBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, Result<DeleteBasketResult>>
{
    public async Task<Result<DeleteBasketResult>> Handle(DeleteBasketCommand command,
        CancellationToken cancellationToken)
    {
        var isDeleted = await repository.DeleteBasketAsync(command.UserName, cancellationToken);
        return isDeleted
            ? Result<DeleteBasketResult>.Success(new DeleteBasketResult(isDeleted), "Basket deleted")
            : Result<DeleteBasketResult>.Failure(Error.NotFound(""));
    }
}
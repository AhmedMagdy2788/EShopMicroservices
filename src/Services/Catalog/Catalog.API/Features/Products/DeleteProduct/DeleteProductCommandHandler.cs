namespace Catalog.API.Features.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<Result<Unit>>;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product id cannot be empty");
    }
}

public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Deleting product with Command {@Command}", command);
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                logger.LogWarning("Product not found: {ProductId}", command.Id);
                return Result<Unit>.Failure(Error.NotFound("Product does not exist"));
            }

            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product Deleted: {ProductId}", command.Id);
            return Result<Unit>.Success(Unit.Value, $"Product '{product.Name}' Deleted");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while deleting product with command: {@command}", command);
            throw;
        }
    }
}
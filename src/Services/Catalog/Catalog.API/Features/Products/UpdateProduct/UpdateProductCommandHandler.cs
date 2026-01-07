namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(Guid id, Product Product) : ICommand<Result<Unit>>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product id cannot be empty");
        RuleFor(c => c.Product.Id).Equal(c => c.id).WithMessage("Id does not match with product id.");
        RuleFor(c => c.Product.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(c => c.Product.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(c => c.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(c => c.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating product with Command {@Command}", command);
            // 1. Load the existing product
            var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
            if (product == null)
            {
                logger.LogWarning("Product not found: {ProductId}", command.Product.Id);
                return Result<Unit>.Failure(Error.NotFound("Product does not exist"));
            }

            // 2. Modify the product
            product.Name = command.Product.Name;
            product.Description = command.Product.Description;
            product.Price = command.Product.Price;
            product.Categories = command.Product.Categories;

            // 3. Update in session (important for lightweight sessions!)
            session.Update(product);

            // 4. Save changes
            await session.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product updated: {ProductId}", command.Product.Id);
            return Result<Unit>.Success(Unit.Value, "Product updated successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while updating products with command: {@command}", command);
            throw;
            // return Result<Unit>.Failure(
            //     Error.DatabaseError("An error occurred while updating  the products from the database"));
        }
    }
}
namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(Guid id, Product Product) : ICommand<Result<Unit>>;

public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating product with Command {@Command}", command);
            if (command.id != command.Product.Id)
            {
                logger.LogWarning("Id does not match with product id in command {@Command}.", command);
                return Result<Unit>.Failure(Error.BadRequest("Id does not match with product id."));
            }
            // 1. Load the existing product
            var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
            if(product == null)
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
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while updating products with command: {@command}", command);
            return Result<Unit>.Failure(
                Error.DatabaseError("An error occurred while updating  the products from the database"));
        }
    }
}
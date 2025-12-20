namespace Catalog.API.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<Result<Product>>;

public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("GetProductByIdQueryHandler. handle called with {@Query}", query);

            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product != null) return Result<Product>.Success(product, $"Product with Id: {query.Id} found");
            logger.LogWarning("Product with Id: {ProductId} not found", query.Id);
            return Result<Product>.Failure(
                Error.NotFound($"Product with Id {query.Id} not found")
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching product with Id: {ProductId}", query.Id);
            return Result<Product>.Failure(
                Error.DatabaseError("An error occurred while fetching the product from the database")
            );
        }
    }
}
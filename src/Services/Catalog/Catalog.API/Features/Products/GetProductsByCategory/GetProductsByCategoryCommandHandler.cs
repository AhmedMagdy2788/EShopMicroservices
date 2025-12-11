namespace Catalog.API.Features.Products.GetProductByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<Result<IReadOnlyList<Product>>>;

public class GetProductsByCategoryCommandHandler(IDocumentSession session, ILogger<GetProductsByCategoryCommandHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, Result<IReadOnlyList<Product>>>
{
    public async Task<Result<IReadOnlyList<Product>>> Handle(GetProductsByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching products with Query{@Query}", query);
            var products = await session.Query<Product>().Where(p => p.Categories.Contains(query.Category))
                .ToListAsync(cancellationToken);
            return Result<IReadOnlyList<Product>>.Success(products);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while fetching products with query: {@Query}", query);
            return Result<IReadOnlyList<Product>>.Failure(
                Error.DatabaseError("An error occurred while fetching the products from the database"));
        }
    }
}
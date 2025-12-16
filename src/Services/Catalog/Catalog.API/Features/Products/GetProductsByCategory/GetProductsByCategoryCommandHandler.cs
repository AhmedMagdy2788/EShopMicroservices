namespace Catalog.API.Features.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category, int? PageNumber, int? PageSize)
    : IQuery<Result<IPagedList<Product>>>;

public class GetProductsByCategoryCommandHandler(
    IDocumentSession session,
    ILogger<GetProductsByCategoryCommandHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, Result<IPagedList<Product>>>
{
    public async Task<Result<IPagedList<Product>>> Handle(GetProductsByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching products with Query{@Query}", query);
            var products = await session.Query<Product>().Where(p => p.Categories.Contains(query.Category))
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
            return Result<IPagedList<Product>>.Success(products,
                $"Category '{query.Category}' hold {products.Count} products");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while fetching products with query: {@Query}", query);
            throw;
        }
    }
}
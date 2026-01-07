namespace Catalog.API.Features.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<Result<IPagedList<Product>>>;

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, Result<IPagedList<Product>>>
{
    public async Task<Result<IPagedList<Product>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);
        var products = await session.Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return Result<IPagedList<Product>>.Success(products,
            $"All products count is {products.TotalItemCount} products");
    }
}